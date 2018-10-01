using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipboardWeaponHolder : PlanExecuteBehaviour {
    [System.Serializable]
    public class ShipboardWeaponSlot
    {
        public bool active;
        [HideInInspector]
        public bool triggered;
        public ShipboardWeapon weapon;
        public float triggerAfter = 0;
        public float ceaseAfter = 1;
    }
    public List<ShipboardWeaponSlot> weaponSlots;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var curSlot in weaponSlots)
        {
            if (!curSlot.active)
            {
                curSlot.weapon.FireCease();
            }
            if (curSlot.active)
            {
                if (!curSlot.triggered && curSlot.triggerAfter < GameManager.instance.t)
                {
                    Debug.Log("Fire after" + GameManager.instance.t);
                    curSlot.weapon.FireStart();
                    curSlot.triggered = true;
                }
                if(curSlot.ceaseAfter < GameManager.instance.t)
                {
                    curSlot.weapon.FireCease();
                }
            }
        }
	}

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        foreach(var curSlot in weaponSlots)
        {
            curSlot.triggered = false;
        }
    }
}
