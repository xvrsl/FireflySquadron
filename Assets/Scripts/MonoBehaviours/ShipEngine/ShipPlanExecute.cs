using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlanExecute : PlanExecuteBehaviour
{
    Rigidbody2D _rigidbody;
    new Rigidbody2D rigidbody
    {
        get
        {
            if(_rigidbody != null)
                return _rigidbody;
            else
            {
                _rigidbody = GetComponent<Rigidbody2D>();
                return _rigidbody;
            }
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnExecutePhaseStart()
    {
        rigidbody.simulated = true;
    }

    public override void OnPlanPhaseStart()
    {
        rigidbody.simulated = false;
    }
}
