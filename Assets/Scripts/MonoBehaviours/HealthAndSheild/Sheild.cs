using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : PlanExecuteBehaviour {
    public float maxSheild = 100;
    public float sheild = 100;
    public float sheildRate
    {
        get
        {
            return sheild / maxSheild;
        }
    }
    public float sheildRegenerationCoolDown = 2;
    float sheildRegenerationCoolDownBuffer;
    public float sheildRegenerationRate = 5;

    [Header("Efficiency")]
    public AnimationCurve sheildEfficiencyCurve = AnimationCurve.Constant(0,1,0.5f);
    public float sheildEfficiency
    {
        get
        {
            return sheildEfficiencyCurve.Evaluate(sheildRate);
        }
    }

    public float physicalBlockFactor = 1;
    public float explosionBlockFactor = 0.5f;
    public float heatBlockFactor = 0.1f;

    public float GetBlockEfficiency(Damage.DamageType type)
    {
        switch (type)
        {
            case Damage.DamageType.physical:
                return physicalBlockFactor;
            case Damage.DamageType.heat:
                return heatBlockFactor;
            case Damage.DamageType.explosion:
                return explosionBlockFactor;
            default:
                return 0;
        }
    }
    public Damage TakeDamageAndGetPierce(Damage damage)
    {
        sheildRegenerationCoolDownBuffer = sheildRegenerationCoolDown;

        float blockedDamage = damage.value * sheildEfficiency * GetBlockEfficiency(damage.damageType);
        damage.value = damage.value - blockedDamage;
        sheild -= blockedDamage;
        if(sheild <= 0)
        {
            sheild = 0;
        }
        return damage;
    }
    private void FixedUpdate()
    {
        if(execute)
        {
            if (sheildRegenerationCoolDownBuffer > 0)
            {
                sheildRegenerationCoolDownBuffer -= Time.fixedDeltaTime;
            }
            if (sheildRegenerationCoolDownBuffer <= 0)
            {
                sheild += sheildRegenerationRate * Time.fixedDeltaTime;
            }
        }
    }
}
