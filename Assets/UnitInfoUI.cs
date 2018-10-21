using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : ControlPanelUI{
    public UnitIdentity target;
    public Image masterImage;
    public Text masterName;
    public Text unitName;
    public Text makerName;
    public Text modelName;

    public void Initialize(UnitIdentity target)
    {
        if(target == null)
        {
            hidden = true;
            return;
        }
        else
        {
            this.target = target;
            masterName.text = target.master.playerName;
            unitName.text = target.unitName;
            makerName.text = target.makerName;
            modelName.text = target.modelName;
        }
    }
	// Use this for initialization
	void Start () {
        hidden = true;
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            hidden = true;
        }
	}
}
