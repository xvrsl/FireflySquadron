using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnginePredictionVisualizer : MonoBehaviour {
    public ShipEngine target;
    public GameObject beaconPrefab;

    [Range(1, 10)]
    public float totalTime = 5f;
    [Range(1,10)]
    public int count = 4;
    
    public bool alwaysVisualizeOnPlan;

	// Use this for initialization
	void Start () {
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
        while(transform.childCount < count)
        {
            GameObject.Instantiate(beaconPrefab, this.transform);
        }
        //Deactivate all child
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void Visualize()
    {
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
                Transform curBeacon = transform.GetChild(i);
                curBeacon.position = curResult.position;
                curBeacon.rotation = Quaternion.Euler(0, 0, curResult.rotation);

                curBeacon.gameObject.SetActive(true);
            }
            else
            {
                ShipEngine.EngineStatus curResult = result[result.Length-1];
                Transform curBeacon = transform.GetChild(i);
                curBeacon.position = curResult.position;
                curBeacon.rotation = Quaternion.Euler(0, 0, curResult.rotation);

                curBeacon.gameObject.SetActive(true);
            }
        }

    }
}
