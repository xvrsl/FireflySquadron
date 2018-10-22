using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ControlPanelUI))]
public class ControlPanelUIHider : MonoBehaviour {
    RectTransform rect;
    ControlPanelUI ui;
    public float hiddenX=-90;
    public float normalX=90;
    public float smooth =0.5f;
    
	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        ui = GetComponent<ControlPanelUI>();
	}
	
	// Update is called once per frame
	void Update () {
        float targetX;
        targetX = ui.hidden? hiddenX:normalX;
        Vector3 pos = this.rect.localPosition;
        float newX = Mathf.Lerp(pos.x, targetX,smooth);
        pos.x = newX;
        this.rect.localPosition = pos;
    }
}
