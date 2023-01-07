using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatUnit : Unit, IMultipleUnitAction
{
    public event Action<IMultipleUnitAction> OnUnitActionRequired;
    public event Action OnUnitActionFinished;

    [SerializeField] private UnitActionManager unitActionManager;

    #region UnityMethods

    protected override void Start()
    {
        base.Start();
        OnUnitActionRequired += ActionManager.Instance.AssignUnitToAction;
        unitActionManager.OnActionFinished += ResetAction;
    }

    #endregion
    
    #region IMultipleUnitAction

    public Queue<Vector3> GetUnitDestinations()
    {
        Queue<Vector3> destinations = UnitBuilding.GetActionDestinations(unitActionManager.GetCurrentAction());
        destinations.Enqueue(GetUAMLocation());
        return destinations;
    }

    public UnitType GetUnitType()
    {
        return UnitType.Logistic;
    }

    public Vector3 GetUAMLocation()
    {
        return unitActionManager.transform.position;
    }

    #endregion

    public void StartAction(UnitActionType type, UnitActionItem item, Action callback)
    {
        var action = new KeyValuePair<UnitActionType, UnitActionItem>(type, item);
        unitActionManager.SetUnitAction(action, callback);
        OnUnitActionRequired?.Invoke(this);
    }

    private void ResetAction()
    {
        OnUnitActionFinished?.Invoke();
        OnUnitActionFinished = null;
    }
}
