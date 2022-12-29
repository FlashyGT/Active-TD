using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour, IUnitAction
{
    [SerializeField] private UnitActionManager unitActionManager;

    #region IUnitAction

    public event Action<IUnitAction> OnUnitActionRequired;
    public event Action OnUnitActionFinished;

    public Queue<Vector3> GetUnitDestinations()
    {
        Queue<Vector3> destinations = new();
        destinations.Enqueue(GetUAMLocation());
        return destinations;
    }

    public UnitType GetUnitType()
    {
        throw new NotImplementedException();
    }

    public Vector3 GetUAMLocation()
    {
        return unitActionManager.transform.position;
    }

    #endregion
}