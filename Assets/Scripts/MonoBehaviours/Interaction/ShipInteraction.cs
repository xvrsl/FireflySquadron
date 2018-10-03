using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShipInteraction : PlanExecuteBehaviour {
    [Header("References")]
    public Player master;
    public ShipEnginePredictionVisualizer visualizer;
    [HideInInspector]
    public bool onlyOperateMastersShip = true; 
    public ShipEngine interactingTarget;
    public Transform shipTransform
    {
        get
        {
            return interactingTarget.transform;
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
            ShipEngine clickedEngine;
            if (CheckIfMouseOnShip(out clickedEngine))
            {
                SetTarget(clickedEngine);
                ToggleTargetBrake(); 
            }
            else
            {
                SetTarget(null);
            }
        }

        if (Input.GetMouseButtonDown(0) && !execute)
        {
            ShipEngine clickedEngine;
            if (CheckIfMouseOnShip(out clickedEngine))
            {
                SetTarget(clickedEngine);
                dragging = true;
            }
            else
            {
                SetTarget(null);
            }
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
        }
        if (dragging)
        {
            if(interactingTarget != null)
            {
                MouseInteraction();
            }
        }
    }

    private void MouseInteraction()
    {
        if(onlyOperateMastersShip && interactingTarget.master != master)
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
       
        float dist = delta.magnitude - accelerationMinDist;
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
        interactingTarget.accelerateHandle = accelerateHandle;
        interactingTarget.steerHandle = steerHandle;
    }

    bool CheckIfMouseOnShip(out ShipEngine ship)
    {
        ship = null;
        Camera mainCamera = Camera.main;
        Vector3 worldMousePoint = mainCamera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * mainCamera.transform.position.z);
        RaycastHit2D hit = Physics2D.CircleCast(worldMousePoint, mouseDownRadius, Vector2.zero);
        if (hit.collider != null)
        {
            ShipEngine clickedEngine = hit.collider.GetComponent<ShipEngine>();
            if(clickedEngine != null)
            {
                ship = clickedEngine;
                return true;
            }
        }
        return false;  
    }

    void ToggleTargetBrake()
    {
        if (interactingTarget != null)
        {
            interactingTarget.brake = !interactingTarget.brake;
        }
    }

    void SetTarget(ShipEngine target)
    {
        interactingTarget = target;
        if (target != null && target.master != this.master && !GameManager.instance.gameSettings.othersShipEngineOperationVisible)
        {
            visualizer.SetTarget(null);
        }
        else
        {
            visualizer.SetTarget(target);
        }
    }
}
