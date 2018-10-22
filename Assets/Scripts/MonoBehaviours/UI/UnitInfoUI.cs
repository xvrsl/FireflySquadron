using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : ControlPanelUI{
    public Unit targetUnit;
    public UnitIdentity target;
    public Image masterImage;
    public Text masterName;
    public Text unitName;
    public Text makerName;
    public Text modelName;
    public Slider healthBar;
    public Slider sheildBar;
    public Text healthBarText;
    public Text sheildBarText;

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
            masterImage.color = target.master.primaryColor;
            targetUnit = target.GetComponent<Unit>();
            hidden = false;
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
        if (!hidden)
        {
            //Refresh health bar and sheild bar
            Health health = targetUnit.health;
            Sheild sheild = targetUnit.sheild;

            float health01 = health.healthRate;
            float sheild01 = sheild.sheildRate;
            healthBar.value = health01;
            sheildBar.value = sheild01;
            float healthMax = health.maxHealth;
            float healthValue = health.health;
            float sheildMax = sheild.maxSheild;
            float sheildValue = sheild.sheild;

            healthBarText.text = "" + (int)healthValue + "/" + (int)healthMax;
            sheildBarText.text = "" + (int)sheildValue + "/" + (int)sheildMax;
        }
	}

}
