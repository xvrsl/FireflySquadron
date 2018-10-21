using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public UnitIdentity identity;
    public ShipEngine engine;
    public ShipboardWeaponHolder weaponHolder;
    public Health health;
    public Sheild sheild;
    public Player master
    {
        get
        {
            return identity.master;
        }
    }

	// Use this for initialization
	void Start () {
        if(identity == null)
            identity = GetComponent<UnitIdentity>();
        if(engine == null)
            engine = GetComponent<ShipEngine>();
        if(weaponHolder == null)
            weaponHolder = GetComponent<ShipboardWeaponHolder>();
        if(health == null)
            health = GetComponent<Health>();
        if(sheild == null)
            sheild = GetComponent<Sheild>();
	}
	
}
