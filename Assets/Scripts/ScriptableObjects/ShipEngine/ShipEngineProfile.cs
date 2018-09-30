using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEngineProfile",menuName = "Profiles/EngineProfile")]
public class ShipEngineProfile : ScriptableObject {
    public static AnimationCurve defaultAnimationCurve
    {
        get
        {
            AnimationCurve result = new AnimationCurve();
            result.AddKey(0.8f, 1f);
            result.AddKey(1.0f, 0f);
            return result;
        }
    }
    public string discription = "Ship engine";
    public float maxSpeed = 50f;
    public AnimationCurve accelerationDecay = defaultAnimationCurve;
    public float maxAcceleratePower = 10;
    public float brakeDrag = 1;
    public float sideVelocityDrag = 5;
    public float maxSteer = 100;
}
