﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Unit))]
public class Health : MonoBehaviour {
    public Player master;

    public Sheild sheild;
    public float maxHealth = 100;
    public float health = 100;
    public float healthRate
    {
        get
        {
            return health / maxHealth;
        }
    }

    void Initialize()
    {
        if(master == null)
        {
            var id = GetComponent<UnitIdentity>();
            if(id != null)
            {
                master = id.master;
            }

        }
        health = maxHealth;
    }

    private void Start()
    {
        Initialize();
    }

    public void TakeDamage(Damage damage)
    {
        damage = new Damage(damage);
        if(sheild != null)
        {
            damage = sheild.TakeDamageAndGetPierce(damage);
        }

        health -= damage.value;
        if(health <= 0)
        {
            HealthZeroedOut();
        }
    }
    public void TakeDamage(List<Damage> damages)
    {
        foreach(var cur in damages)
        {
            TakeDamage(cur);
        }
    }
    public void TakeHeal(float heal)
    {
        health += heal;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }
    public void HealthZeroedOut()
    {
        Destroy(this.gameObject);
    }
}
