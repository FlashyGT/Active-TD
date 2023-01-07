using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHouse : MonoBehaviour, ISingleUnitAction
{
    [SerializeField] private UnitActionManager unitActionManager;

    #region ISingleUnitAction

    public Vector3 GetUAMLocation()
    {
        return unitActionManager.transform.position;
    }

    #endregion
}