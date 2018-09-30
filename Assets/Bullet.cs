using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : PlanExecuteBehaviour {
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
    public float damage;
    public float lifeTime = 5;
    float age;
    bool executing;

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
        if(executing)
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
        executing = false;
        rigidbody.simulated = false;
    }

    public override void OnExecutePhaseStart()
    {
        executing = true;
        rigidbody.simulated = true;
    }
}
