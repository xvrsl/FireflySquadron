using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSlotUI : ControlPanelUI
{
    public ShipboardWeaponHolder targetWeaponHolder;
    public Button[] slotChangingButtons;
    public int index;
    public bool weaponSlotValid
    {
        get
        {
           return (targetWeaponHolder != null) && (targetWeaponHolder.weaponSlots.Count > index);
        }
    }
    public ShipboardWeaponHolder.ShipboardWeaponSlot target
    {
        get
        {
            if(weaponSlotValid)
            {
                return targetWeaponHolder.weaponSlots[index];
            }
            return null;
        }
    }
    public Text weaponName;
    public Slider activationSlider;
    bool activation
    {
        get
        {
            return activationSlider.value == 1;
        }
    }
    public RangeSlider fireCeaseSlider;
    bool valueDirty;

    public void Initialize(ShipboardWeaponHolder target)
    {
        this.targetWeaponHolder = target;
        this.index = 0;
        if(!weaponSlotValid)
        {
            hidden = true;
        }
        else
        {
            hidden = false;
            int slotChangingButtonsCount = slotChangingButtons.Length;
            int targetWeaponSlotsCount = target.weaponSlots.Count;
            if(targetWeaponSlotsCount < 2)
            {
                for (int i = 0; i < slotChangingButtonsCount; i++)
                {
                    var curButton = slotChangingButtons[i];
                    curButton.gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < slotChangingButtonsCount; i++)
                {
                    var curButton = slotChangingButtons[i];
                    curButton.gameObject.SetActive(i < targetWeaponSlotsCount);
                }
            }
            RenewValues();
        }
    }
    public void SetIndex(int index)
    {
        this.index = index;
        ShipEnginePredictionVisualizer.instance.weaponHolder = targetWeaponHolder;
        ShipEnginePredictionVisualizer.instance.visualizeWeaponIndex = index;
        RenewValues();
    }
    public void ApplyValues()
    {
        target.active = activation;
        target.triggerAfter = fireCeaseSlider.smallValue;
        target.ceaseAfter = fireCeaseSlider.bigValue;
        valueDirty = false;
    }
    public void RenewValues()
    {
        weaponName.text = target.weapon.weaponName;
        activationSlider.value = target.active?1:0;
        fireCeaseSlider.SetValue(target.triggerAfter, target.ceaseAfter);
    }

    public bool CheckValueChangedOutside()
    {
        if(target.active != activation
            || target.triggerAfter != fireCeaseSlider.smallValue
            || target.ceaseAfter != fireCeaseSlider.bigValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SliderValueChanged()
    {
        valueDirty = true;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if(weaponSlotValid)
        {
            if (valueDirty)
            {
                ApplyValues();
            }
            if (CheckValueChangedOutside())
            {
                RenewValues();
            }
        }
        else
        {
            hidden = true;
        }
        

	}
}
