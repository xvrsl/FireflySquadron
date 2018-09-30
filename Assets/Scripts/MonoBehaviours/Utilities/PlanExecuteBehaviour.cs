using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlanExecuteBehaviour : MonoBehaviour {
    public abstract void OnPlanPhaseStart();
    public abstract void OnExecutePhaseStart();
    private void Awake()
    {
        GameManager.instance.RegisterPlanExecuteBehaviour(this);
        if(GameManager.instance.gamePhase == GameManager.GamePhase.plan)
        {
            OnPlanPhaseStart();
        }
        else
        {
            OnExecutePhaseStart();
        }
        
    }
    private void OnDestroy()
    {
        GameManager.instance.LogoutPlanExecuteBehaviour(this);
    }
}
