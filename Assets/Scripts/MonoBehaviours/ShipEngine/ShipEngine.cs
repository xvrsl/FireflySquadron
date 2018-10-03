using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : PlanExecuteBehaviour {
    public Player master;
    Rigidbody2D _rigidBody;
    public new Rigidbody2D rigidbody
    {
        get
        {
            if(_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
            }
            return _rigidBody;
        }
    }
    public ShipEngineProfile profile;
    [Header("Engine Status")]
    public float maxSpeed = 50f;
    public AnimationCurve accelerationDecay;
    public float maxAcceleratePower = 10f;
    public float brakeDrag = 10f;
    public float sideVelocityDrag = 10f;
    public float maxSteer = 50f;
    [Header("Operation")]
    [Range(0, 1)]
    public float accelerateHandle;
    [Range(-1, 1)]
    public float steerHandle;
    public bool brake;

    [Header("Stored Informations")]
    public Vector2 velocityStored;
    public float angularVelocityStored;

    public Vector3 position
    {
        get
        {
            return rigidbody.position;
        }
    }
    public float rotation
    {
        get
        {
            return rigidbody.rotation;
        }
    }
    public Vector3 forward
    {
        get
        {
            return this.transform.up;
        }
    }
    public Vector3 right
    {
        get
        {
            return this.transform.right;
        }
    }
    public Vector3 velocity
    {
        get
        {
            if (!execute)
            {
                return velocityStored;
            }
            return rigidbody.velocity;
        }
    }
    public Vector3 sideVelocity
    {
        get
        {
            return right * Vector3.Dot(velocity, right);
        }
    }
    public float mass
    {
        get
        {
            return rigidbody.mass;
        }
    }
    public float accelerationForce
    {
        get
        {
            float power = accelerateHandle * maxAcceleratePower * accelerationDecay.Evaluate(velocity.magnitude / maxSpeed);
            return power;
        }
    }
    public float steer
    {
        get
        {
            return maxSteer * steerHandle;
        }
    }

    public struct EngineStatus
    {
        public Vector3 position;
        public float rotation;
        public Vector3 velocity;
        public float mass;
        public float accelerationForce;
        public bool brake;
        public float brakeDrag;
        public float sideVelocityDrag;
        public float steer;
        public float fixedDeltaTime;
        public Vector3 right
        {
            get
            {
                return Quaternion.Euler(0, 0, 90) * forward;
            }
        }
        public Vector3 sideVelocity
        {
            get
            {
                return right * Vector3.Dot(velocity, right);
            }
        }

        public EngineStatus(Vector3 position, float rotation, Vector3 velocity, float mass, float accelerationForce, bool brake, float brakeDrag,float sideVelocityDrag, float steer, float fixedDeltaTime)
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = velocity;
            this.mass = mass;
            this.accelerationForce = accelerationForce;
            this.brake = brake;
            this.brakeDrag = brakeDrag;
            this.sideVelocityDrag = sideVelocityDrag;
            this.steer = steer;
            this.fixedDeltaTime = fixedDeltaTime;
        }
        public EngineStatus(EngineStatus copyFrom)
        {
            this.position = copyFrom.position;
            this.rotation = copyFrom.rotation;
            this.velocity = copyFrom.velocity;
            this.mass = copyFrom.mass;
            this.accelerationForce = copyFrom.accelerationForce;
            this.brake = copyFrom.brake;
            this.brakeDrag = copyFrom.brakeDrag;
            this.sideVelocityDrag = copyFrom.sideVelocityDrag;
            this.steer = copyFrom.steer;
            this.fixedDeltaTime = copyFrom.fixedDeltaTime;
        }

        public Vector3 forward
        {
            get
            {
                return Quaternion.Euler(0,0,rotation) * Vector3.up;
            }
        }

        public static EngineStatus PredictNextFixedUpdate(EngineStatus initialStatus)
        {
            EngineStatus result = new EngineStatus(initialStatus);
            Quaternion steerRotation = Quaternion.Euler(0, 0, initialStatus.steer * result.fixedDeltaTime);
            result.rotation += initialStatus.steer* result.fixedDeltaTime;
            result.velocity = steerRotation * result.velocity;//rotate

            if (initialStatus.brake)
            {
                result.velocity = result.velocity * (1 - result.fixedDeltaTime * initialStatus.brakeDrag);
            }
            else
            {
                result.velocity += initialStatus.accelerationForce / initialStatus.mass *  result.fixedDeltaTime * result.forward;
                result.velocity -= (result.fixedDeltaTime * result.sideVelocityDrag * result.sideVelocity);
            }
            result.position += result.velocity * initialStatus.fixedDeltaTime;
            return result;
        }
        public static EngineStatus[] Predict(EngineStatus initialStatus, float totalTime,float deltaTime)
        {
            if(deltaTime <= 0)
            {
                Debug.LogWarning("Delta Time set to zero, unable to predict engine status");
            }
            return Predict(initialStatus, (int)1, (int)Mathf.Round(totalTime/deltaTime));
        }

        public static EngineStatus[] Predict(EngineStatus initialStatus,int stepLength, int count)
        {
            EngineStatus[] result = new EngineStatus[count];
            EngineStatus currentStatus = initialStatus;
            for(int i = 0; i < count; i++) 
            {
                for(int j = 0; j < stepLength; j++)
                {
                    currentStatus = PredictNextFixedUpdate(currentStatus);
                }
                result[i] = currentStatus;
            }
            return result;
        }

        public static EngineStatus[] Predict(EngineStatus initialStatus, float stepTime,int count, float deltaTime)
        {
            if(deltaTime <= 0)
            {
                Debug.LogWarning("Delta Time set to zero, unable to predict engine status");
                return null;
            }
            int stepLength = (int)Mathf.Round( stepTime / deltaTime );
            return Predict(initialStatus, stepLength, count);
        }
    }
    public EngineStatus engineStatus
    {
        get
        {
            return new EngineStatus(position, rotation, velocity, mass, accelerationForce,brake,brakeDrag,sideVelocityDrag, steer,Time.fixedDeltaTime);
        }
    }

	// Use this for initialization
	void Start () {

        if (profile != null)
        {
            LoadProfile(profile);
        }

        var id = GetComponent<UnitIdentity>();
        if(id != null)
        {
            master = id.master;
        }
	}

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
        velocityStored = rigidbody.velocity;
        angularVelocityStored = rigidbody.angularDrag;
        rigidbody.velocity = Vector2.zero ;
        rigidbody.angularVelocity = 0;
    }

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        rigidbody.velocity = velocityStored;
        rigidbody.angularVelocity = angularVelocityStored;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(execute)
        {
            EngineFixedUpdate();
        }
	}

    void EngineFixedUpdate()
    {
        if (!rigidbody.simulated)
        {
            return;
        }
        Quaternion steerRotation = Quaternion.Euler(0, 0, steer * Time.fixedDeltaTime);
        rigidbody.velocity = steerRotation * velocity;//Rotate Velocity
        rigidbody.MoveRotation(rigidbody.rotation + steer * Time.deltaTime);
        
        if(brake)
        {
            rigidbody.drag = brakeDrag;
        }
        else
        {
            rigidbody.drag = 0;
            rigidbody.velocity -=(Vector2)(Time.fixedDeltaTime * sideVelocityDrag * sideVelocity);
            rigidbody.AddForce(accelerationForce * forward, ForceMode2D.Force);
        }
    }

    void LoadProfile(ShipEngineProfile profile)
    {
        this.profile = profile;
        maxSpeed = profile.maxSpeed;
        accelerationDecay = profile.accelerationDecay;
        maxAcceleratePower = profile.maxAcceleratePower;
        brakeDrag = profile.brakeDrag;
        sideVelocityDrag = profile.sideVelocityDrag;
        maxSteer = profile.maxSteer;
    }

}
