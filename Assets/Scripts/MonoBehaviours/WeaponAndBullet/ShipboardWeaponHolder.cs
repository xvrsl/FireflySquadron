using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Unit))]
public class ShipboardWeaponHolder : PlanExecuteBehaviour {
    public Player master;
    [System.Serializable]
    public class ShipboardWeaponSlot
    {
        public bool active;
        [HideInInspector]
        public bool triggered;
        public ShipboardWeapon weapon;
        [Range(0,1)]
        public float triggerAfter = 0;
        [Range(0, 1)]
        public float ceaseAfter = 1;
    }
    public List<ShipboardWeaponSlot> weaponSlots;

	// Use this for initialization
	void Start () {
        if(master == null)
        {
            var id = this.GetComponent<UnitIdentity>();
            if(id != null)
            {
                master = id.master;
            }
        }
        foreach (var curSlot in weaponSlots)
        {
            if(curSlot.weapon.master == null)
            {
                curSlot.weapon.master = master;
            }
        }

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
