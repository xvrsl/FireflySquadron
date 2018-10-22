using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShipInteraction : PlanExecuteBehaviour {
    [Header("References")]
    public Player master;
    public ShipEnginePredictionVisualizer visualizer;
    public ControlPanelsHub controlPanelsHub {
        get
        {
            return ControlPanelsHub.instance;
        }
    }
    [HideInInspector]
    public bool onlyOperateMastersShip = true;
    public Unit target;
    public ShipEngine interactingShipEngine
    {
        get
        {
            if(target == null)
            {
                return null;
            }
            return target.engine;
        }
    }
    public Transform shipTransform
    {
        get
        {
            return interactingShipEngine.transform;
        }
    }
    [Header("Snap")]
    public bool snap = true;
    public float accelerationStep = 1;
    public float steerStep = 1;
    [Header("Interaction Preferences")]
    public float accelerationMinDist = 5f;
    public float accelerationMaxDist = 10f;
    public float brakeMinDist = 2f;
    public float mouseDownRadius = 0.1f;

    public bool dragging;

    // Use this for initialization
    void Start () {
        if(master == null)
        {
            master = GetComponent<Player>();
        }
        if(visualizer == null)
        {
            visualizer = GetComponent<ShipEnginePredictionVisualizer>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) && !execute)
        {

            SetTarget(null);

        }
        if (Input.GetMouseButtonDown(0) && !execute && Input.GetButton("Alt"))
        {
            Unit clickedUnit;
            if (CheckIfMouseOnShip(out clickedUnit))
            {
                SetTarget(clickedUnit);
                ToggleTargetBrake();
            }
        }

            if (Input.GetMouseButtonDown(0))
        {
            Unit clickedUnit;
            if (CheckIfMouseOnShip(out clickedUnit))
            {
                SetTarget(clickedUnit);
                dragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
        }
        if (dragging)
        {
            if(interactingShipEngine != null)
            {
                MouseInteraction();
            }
        }
    }

    private void MouseInteraction()
    {
        if (execute)
        {
            return;
        }
        if(onlyOperateMastersShip && interactingShipEngine.master != master)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Camera mainCamera = Camera.main;
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos - Vector3.forward * mainCamera.transform.position.z);
        Vector3 delta = worldMousePos - shipTransform.position;
        Vector3 forward = shipTransform.up;
        float theta = Quaternion.FromToRotation(forward, delta).eulerAngles.z;
        if(theta > 180)
        {
            theta -= 360;
        }

        bool changeAcceleration = true;
        float dist = delta.magnitude - accelerationMinDist;
        if(dist < 0)
        {
            changeAcceleration = false;
        }
        dist = Mathf.Clamp(dist, 0, accelerationMaxDist);


        if (snap)
        {
            if (accelerationStep > 0)
            {
                float distQuotient = dist / accelerationStep;
                int distStepCount = (int)Mathf.Round(distQuotient);
                dist = accelerationStep * distStepCount;
            }
            if (steerStep > 0)
            {
                float rightQuotient = theta / steerStep;
                int forwardStepCount = (int)Mathf.Round(rightQuotient);
                theta = steerStep * forwardStepCount;
            }
        }

        float accelerateHandle = dist / accelerationMaxDist;
        float steerHandle = theta / 90;
        accelerateHandle = Mathf.Clamp01(accelerateHandle);
        steerHandle = Mathf.Clamp(steerHandle, -1, 1);
        if(changeAcceleration)
        {
            interactingShipEngine.accelerateHandle = accelerateHandle;
        }
        interactingShipEngine.steerHandle = steerHandle;
    }

    bool CheckIfMouseOnShip(out Unit unit)
    {
        unit = null;
        Camera mainCamera = Camera.main;
        Vector3 worldMousePoint = mainCamera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * mainCamera.transform.position.z);
        RaycastHit2D hit = Physics2D.CircleCast(worldMousePoint, mouseDownRadius, Vector2.zero);
        if (hit.collider != null)
        {
            Unit clickedUnit = hit.collider.GetComponent<Unit>();
            if(clickedUnit != null)
            {
                unit = clickedUnit;
                return true;
            }
        }
        return false;  
    }

    void ToggleTargetBrake()
    {
        if (interactingShipEngine != null && interactingShipEngine.master == master)
        {
            interactingShipEngine.brake = !interactingShipEngine.brake;
        }
    }

    void SetTarget(Unit target)
    {
        this.target = target;
        this.controlPanelsHub.SetTarget(target,target != null && target.master == master);
        if (target == null || target.master != this.master && !GameManager.instance.gameSettings.othersShipEngineOperationVisible)
        {
            visualizer.SetTarget(null);
        }
        else
        {
            //Actually Working
            visualizer.SetTarget(target);
        }
    }
}
