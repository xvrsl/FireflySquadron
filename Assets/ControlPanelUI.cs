using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ControlPanelUI : MonoBehaviour {
    [HideInInspector]
    public bool hidden;
    [HideInInspector]
    public Animator animator;
    void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }    
    }
    protected virtual void Update()
    {

        //animator.SetBool("Hidden", hidden);

    }
}
