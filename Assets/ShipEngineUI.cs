using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEngineUI : ControlPanelUI
{
    public ShipEngine target;
    public Slider accelerationSlider;
    public Slider steerSlider;
    public Slider brakeSlider;
    bool valueDirty = false;

    void Initialize()
    {
        if(target != null)
        {
            accelerationSlider.value = target.accelerateHandle;
            steerSlider.value = target.steerHandle;
            brakeSlider.value = target.brake ? 1 : 0;
            valueDirty = false;
        }
        else
        {
            hidden = true;
        }
    }
    public void Initialize(ShipEngine target)
    {
        this.target = target;
        Initialize();
    }
	// Use this for initialization
	void Start () {
        Initialize();
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target != null)
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
	}

    public void ValueChanged()
    {
        valueDirty = true;
    }

    public void ApplyValues()
    {
        target.accelerateHandle = accelerationSlider.value;
        target.steerHandle = steerSlider.value;
        target.brake = brakeSlider.value == 1;
        valueDirty = false;
    }

    public bool CheckValueChangedOutside()
    {
        if(this.accelerationSlider.value != target.accelerateHandle
            || this.steerSlider.value != target.steerHandle
            || (this.brakeSlider.value == 1)!= target.brake)
        {
            return true;
        }
        return false;
    }

    public void RenewValues()
    {
        accelerationSlider.value = target.accelerateHandle;
        steerSlider.value = target.steerHandle;
        brakeSlider.value = target.brake ? 1 : 0;
        valueDirty = false;
    }
}
