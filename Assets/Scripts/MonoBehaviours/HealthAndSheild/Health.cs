using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
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
        health = maxHealth;
    }

    private void Start()
    {
        Initialize();
    }

    public void TakeDamage(Damage damage)
    {
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

    }
}
