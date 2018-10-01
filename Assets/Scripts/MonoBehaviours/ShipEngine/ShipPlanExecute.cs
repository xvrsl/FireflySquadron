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

    [Header("Stored Informations")]
    public Vector2 velocity;
    public float angularVelocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        rigidbody.velocity = velocity;
        rigidbody.angularDrag = angularVelocity;
    }

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
    }
}
