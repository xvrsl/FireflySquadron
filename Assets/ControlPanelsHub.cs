using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelsHub : MonoBehaviour {
    public static ControlPanelsHub instance;
    public UnitInfoUI shipInfoUI;
    public ShipEngineUI shipEngineUI;
    public WeaponSlotUI weaponSlotUI;
    public void SetTarget(Unit target)
    {
        if(target == null)
        {
            shipEngineUI.Initialize(null);
            shipEngineUI.Initialize(null);
            weaponSlotUI.Initialize(null);
            return;
        }
        shipInfoUI.Initialize(target.identity);
        shipEngineUI.Initialize(target.engine);
        weaponSlotUI.Initialize(target.weaponHolder);
    }

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
