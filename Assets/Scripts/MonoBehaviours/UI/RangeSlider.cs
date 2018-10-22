using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RangeSlider : MonoBehaviour {
    public Slider subSliderA;
    public Slider subSliderB;
    public float a
    {
        get
        {
            if(subSliderA == null)
            {
                Debug.LogWarning("Slider not set to an instance");
                return 0;
            }
            return subSliderA.value;
        }
    }
    public float b
    {
        get
        {
            if(subSliderB == null)
            {
                Debug.LogWarning("Slider not set to an instance");
                return 0;
            }
            return subSliderB.value;
        }
    }
    public bool aSmallerThanB
    {
        get
        {
            return a < b;
        }
    }
    public float smallValue
    {
        get
        {
            return aSmallerThanB ? a : b;
        }
    }
    public float bigValue
    {
        get
        {
            return aSmallerThanB ? b : a;
        }
    }
    public Vector2 range
    {
        get
        {
            if (aSmallerThanB)
            {
                return new Vector2(a, b);
            }
            else
            {
                return new Vector2(b, a);
            }
        }
    }
    [Space]
    public UnityEvent onValueChanged;

    public void SetValue(float a, float b)
    {
        subSliderA.value = a;
        subSliderB.value = b;
    }

    public void SubSliderValueChanged()
    {
        onValueChanged.Invoke();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
