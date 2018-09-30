using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipboardWeapon : MonoBehaviour {
    public enum WeaponType
    {
        bullet = 0,
        instant = 1,
        laser = 2
    }
    public float fireRate;
    public float damage;
}
