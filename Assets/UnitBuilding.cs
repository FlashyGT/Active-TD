using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBuilding : MonoBehaviour
{
    // Locations and unit count have to match
    [SerializeField] protected List<Vector3> unitLocations;
    [SerializeField] protected List<Unit> units;

    protected void Start()
    {
        EnableUnits();
    }

    public abstract Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action);
    
    protected void EnableUnits()
    {
        for (int x = 0; x < units.Count; x++)
        {
            Unit unit = units[x];
            Transform unitParent = unit.transform.parent;
            
            unit.UnitBuilding = this;

            unitParent.position = unitLocations[x];
            unitParent.gameObject.SetActive(true);
        }
    }
}
