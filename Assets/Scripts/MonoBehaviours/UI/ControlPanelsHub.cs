using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelsHub : PlanExecuteBehaviour {
    public static ControlPanelsHub instance;
    public UnitInfoUI shipInfoUI;
    public ShipEngineUI shipEngineUI;
    public WeaponSlotUI weaponSlotUI;
    public void SetTarget(Unit target,bool controlable)
    {
        
        if(target == null)
        {
            shipInfoUI.Initialize(null);
            shipEngineUI.Initialize(null);
            weaponSlotUI.Initialize(null);
            return;
        }
        shipInfoUI.Initialize(target.identity);
        shipEngineUI.Initialize(target.engine);
        weaponSlotUI.Initialize(target.weaponHolder);
        shipEngineUI.SetControlability(!execute && controlable);
        weaponSlotUI.SetControlability(!execute && controlable);
    }

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
