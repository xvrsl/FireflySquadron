using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : PlanExecuteBehaviour {
    public Player master;
    Rigidbody2D _rigidbody;

    public new Rigidbody2D rigidbody
    {
        get
        {
            if(_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody2D>(); 
            }
            return _rigidbody;
        }
    }
    public float flyingSpeed;
    public List<Damage> damages;

    public float lifeTime = 5;
    float age;
    public bool destroyOnContact = true;
    public bool destroyOnDealDamage = true;

    public void Initialize()
    {
        age = 0;
        rigidbody.velocity = this.transform.up * flyingSpeed;
    }

	// Use this for initialization
	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if(execute)
        {
            age += Time.deltaTime;
        }
        if(age >= lifeTime)
        {
            Destroy();
        }
        //rigidbody.velocity = this.transform.up * flyingSpeed;
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
        execute = false;
        rigidbody.simulated = false;
    }

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        execute = true;
        rigidbody.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            //Friendly fire check
            if(!GameManager.instance.gameSettings.friendlyDamage && targetHealth.master == this.master)
            {
                //Deal no damage
            }
            else
            {
                targetHealth.TakeDamage(damages);
                if (destroyOnDealDamage)
                {
                    Destroy();
                    return;
                }
            }
            //Deal Damage
        }
        if (destroyOnContact)
        {
            Destroy();
            return;
        }
    }
}
