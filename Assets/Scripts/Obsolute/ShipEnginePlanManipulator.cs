using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnginePlanManipulator : PlanExecuteBehaviour {
    public ShipEngine shipEngine;
    [System.Serializable]
    public class PlanUnit
    {
        public bool enable = true;
        public float totalTime;
        [Range(0,1)]
        public float accelerationHandle;
        [Range(-1,1)]
        public float steerHandle;
        public bool brake;
    }
    public List<PlanUnit> plan;
    int index;
    float estimatedTime;

	// Use this for initialization
	void Start () {
		if(shipEngine == null)
        {
            shipEngine = GetComponent<ShipEngine>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.instance.isExecutePhase)
        {
            if(index < plan.Count && GameManager.instance.executePhaseTimer > estimatedTime)
            {
                ExecuteNextPlan();
            }
        }
	}

    public void ExecuteNextPlan()
    {
        index++;
        if (plan.Count > index)
        {
            if(plan[index].enable)
            {
                shipEngine.accelerateHandle = plan[index].accelerationHandle;
                shipEngine.steerHandle = plan[index].steerHandle;
                shipEngine.brake = plan[index].brake;
            }
            estimatedTime += plan[index].totalTime;
        }
    }

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
    }

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        index = -1;
        ExecuteNextPlan();
    }
}
