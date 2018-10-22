using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlanExecuteBehaviour
{
    public string playerName = "Captain Adam";
    public string discription = "A captain who is very cool";
    public Sprite avatar = null;
    public Color primaryColor = Color.cyan;
    public Color secondaryColor = Color.blue;
    public List<Unit> units = new List<Unit>();

    public void Register(Unit unit)
    {
        if (units.Contains(unit))
        {
            return;
        }
        else
        {
            units.Add(unit);
        }
    }

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
        RefreshUnitList();
    }

    public void RefreshUnitList()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
            {
                units.RemoveAt(i);
                i--;
            }
        }
    }
}
