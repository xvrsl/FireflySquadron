using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Damage {
    public float value;
    public enum DamageType
    {
        physical = 0,
        explosion = 1,
        heat = 2
    }
    public DamageType damageType = DamageType.physical;

    public Damage(float value, DamageType damageType)
    {
        this.value = value;
        this.damageType = damageType;
    }

    public Damage(Damage original)
    {
        this.value = original.value;
        this.damageType = original.damageType;
    }
    public static List<Damage> GetSingleDamageList(float value,DamageType damageType)
    {
        List<Damage> result = new List<Damage>();
        result.Add(new Damage(value, damageType));
        return result;
    }
}
