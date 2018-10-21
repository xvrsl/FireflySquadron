using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipboardWeapon : PlanExecuteBehaviour {
    public Player master;
    public string weaponName = "Unknown";
    public string discription = "Unknown";
    public bool fire;
    public float maxMagazine
    {
        get
        {
            return GetMaxMagazine();
        }
    }
    public float magazine
    {
        get
        {
            return GetMagazine();
        }
    }
    public float magazineStatus
    {
        get
        {
            if(maxMagazine > 0)
            {
                return magazine / maxMagazine;
            }
            else
            {
                return 0;
            }
        }
    }

    public abstract float GetMaxMagazine();
    public abstract float GetMagazine();

    public abstract void OnFire();
    public abstract void OnFireStart();
    public abstract void OnFireCease();

    public void FireStart()
    {
        OnFireStart();
        fire = true;
    }
    public void FireCease()
    {
        OnFireCease();
        fire = false;
    }

    protected virtual void Update()
    {
        if (fire)
        {
            OnFire();
        }
    }

}