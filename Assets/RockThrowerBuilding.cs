using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowerBuilding : UnitBuilding, ISingleUnitAction
{
    [SerializeField] private UnitActionManager unitActionManager;
    [SerializeField] private FoodHouse foodHouse;

    #region UnitBuilding

    public override Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        Queue<Vector3> destinations = new();

        switch (action.Value)
        {
            case UnitActionItem.Ammo:
                destinations.Enqueue(GetUAMLocation());
                break;
            case UnitActionItem.Food:
                destinations.Enqueue(foodHouse.GetUAMLocation());
                break;
        }

        return destinations;
    }

    #endregion
    
    #region ISingleUnitAction

    public Vector3 GetUAMLocation()
    {
        return unitActionManager.transform.position;
    }

    #endregion
}