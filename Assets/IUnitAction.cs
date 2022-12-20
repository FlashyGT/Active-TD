using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAction
{
    public event Action<IUnitAction> OnUnitActionRequired;

    // After action finished remember to clear it setting it to null,
    // otherwise event will fire off multiple times for subscribers
    public event Action OnUnitActionFinished;

    public Queue<Vector3> GetUnitDestinations();

    public UnitType GetUnitType();

    // UAM - Unit Action Manager (colliders location)
    public Vector3 GetUAMLocation();
}