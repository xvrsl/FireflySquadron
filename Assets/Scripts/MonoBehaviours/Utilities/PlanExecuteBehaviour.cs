using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlanExecuteBehaviour : MonoBehaviour {
    public bool execute;
    public virtual void OnPlanPhaseStart()
    {
        execute = false;
    }
    public virtual void OnExecutePhaseStart()
    {
        execute = true;
    }
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
