using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Unit))]
public class UnitIdentity : MonoBehaviour {
    public Player master;
    public enum UnitType
    {
        locomotive = 0,
        building = 1
    }
    public UnitType type;
    public string unitName = "Unnamed";
    public string makerName = "Unknown Factory";
    public string modelName = "Unkonwn Type";

}
