using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnginePredictionVisualizer : MonoBehaviour {
    public ShipEngine target;
    public ShipboardWeaponHolder weaponHolder;
    public int visualizeWeaponIndex = 0;
    public Transform movementBeaconParent;
    public GameObject fireBeacon;
    public GameObject ceaseBeacon;
    public GameObject beaconPrefab;
    public GameObject fireBeaconPrefab;
    public GameObject ceaseBeaconPrefab;

    public float totalTime
    {
        get
        {
            return GameManager.instance.gameSettings.executeTime;
        }
    }
    [Range(1,10)]
    public int count = 4;
    
    public bool alwaysVisualizeOnPlan;

	// Use this for initialization
	void Start () {
        if(movementBeaconParent == null)
        {
            movementBeaconParent = this.transform;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(GameManager.instance.gamePhase == GameManager.GamePhase.plan)
        {
            Visualize();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            Visualize();
        }
        
	}

    void InitializeVisualization()
    {
        while(movementBeaconParent.childCount < count)
        {
            GameObject.Instantiate(beaconPrefab, movementBeaconParent);
        }
        //Deactivate all child
        for(int i = 0; i < movementBeaconParent.childCount; i++)
        {
            movementBeaconParent.GetChild(i).gameObject.SetActive(false);
        }

        if(fireBeacon == null)
        {
            fireBeacon = GameObject.Instantiate(fireBeaconPrefab, this.transform);
        }
        if(ceaseBeacon == null)
        {
            ceaseBeacon = GameObject.Instantiate(ceaseBeaconPrefab, this.transform);
        }
    }
    public void Visualize()
    {
        if(target== null)
        {
            return;
        }
        ShipEngine.EngineStatus engineStatus= target.engineStatus;
        var result = ShipEngine.EngineStatus.Predict(engineStatus,totalTime,Time.fixedDeltaTime);
        InitializeVisualization();
        float stepTime = totalTime / count;
        int stepCount = (int)Mathf.Round(stepTime / Time.fixedDeltaTime);
        for(int i = 0; i < count; i++)
        {
            int index = (i+1) * stepCount;
            if(index < result.Length)
            {
                ShipEngine.EngineStatus curResult = result[index];
                Transform curBeacon = movementBeaconParent.GetChild(i);
                curBeacon.position = curResult.position;
                curBeacon.rotation = Quaternion.Euler(0, 0, curResult.rotation);

                curBeacon.gameObject.SetActive(true);
            }
            else
            {
                ShipEngine.EngineStatus curResult = result[result.Length-1];
                Transform curBeacon = movementBeaconParent.GetChild(i);
                curBeacon.position = curResult.position;
                curBeacon.rotation = Quaternion.Euler(0, 0, curResult.rotation);

                curBeacon.gameObject.SetActive(true);
            }
        }
        //Weapon Visualize
        if(weaponHolder != null)
        {
            if(visualizeWeaponIndex < weaponHolder.weaponSlots.Count)
            {
                var visualizeSlot = weaponHolder.weaponSlots[visualizeWeaponIndex];
                if (visualizeSlot.active)
                {
                    float fireT = visualizeSlot.triggerAfter;
                    float ceaseT = visualizeSlot.ceaseAfter;
                    int fireIndex = (int)Mathf.Round(totalTime * fireT / Time.fixedDeltaTime);
                    int ceaseIndex = (int)Mathf.Round(totalTime * ceaseT / Time.fixedDeltaTime);
                    fireIndex = Mathf.Clamp(fireIndex, 0, result.Length - 1);
                    ceaseIndex = Mathf.Clamp(ceaseIndex, 0, result.Length - 1);
                    Vector3 firePos = result[fireIndex].position;
                    Vector3 ceasePos = result[ceaseIndex].position;
                    float fireRot = result[fireIndex].rotation;
                    float ceaseRot = result[ceaseIndex].rotation;
                    fireBeacon.transform.position = firePos;
                    fireBeacon.transform.rotation = Quaternion.Euler(0, 0, fireRot);

                    ceaseBeacon.transform.position = ceasePos;
                    ceaseBeacon.transform.rotation = Quaternion.Euler(0, 0, ceaseRot);
                }
            }
        }
    }
    
}
