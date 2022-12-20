using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }

    [SerializeField] private Barricade barricade;
    [SerializeField] private Farm farm;

    [field: SerializeField] public Well Well { get; private set; }

    private readonly Dictionary<UnitType, Queue<Unit>> _units = new();
    private readonly Dictionary<UnitType, Queue<IUnitAction>> _actions = new();

    #region UnityMethods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InitializeData();
    }

    #endregion

    public Vector3 GetEnemyActionDestination(Unit unit)
    {
        IDamageable target;

        if (!barricade.ObjectHealth.IsDead())
        {
            target = barricade;
        }
        else
        {
            target = farm.GetGarden();
        }

        if (target == null)
        {
            // No target, return units position, so it doesn't move
            return unit.transform.position;
        }

        target.OnObjDeath.AddListener(unit.Movement.InitMovement);
        return target.GetAttackPoint();
    }

    public void UnitIsAvailable(UnitType type, Unit unit)
    {
        _units[type].Enqueue(unit);

        // If there are actions for this type of unit in queue then begin
        // that action immediately with the current unit
        if (HasActionsForUnitType(type))
        {
            AssignUnitToAction(_actions[type].Dequeue());
        }
    }

    public void AssignUnitToAction(IUnitAction unitAction)
    {
        UnitType unitType = unitAction.GetUnitType();

        Queue<Unit> units = _units[unitType];

        // If there are no units of this type available, then add the action to a queue
        if (units.Count == 0)
        {
            _actions[unitType].Enqueue(unitAction);
            return;
        }

        Queue<Vector3> destinations = unitAction.GetUnitDestinations();
        Unit unit = units.Dequeue();

        unitAction.OnUnitActionFinished += unit.Action.ResetAction;
        unit.Action.StartAction(destinations);
    }

    public void RemoveAction(IUnitAction action)
    {
        UnitType type = action.GetUnitType();
        _actions[type] = GameManager.Instance.RemoveItemFromQueue(action, _actions[type]);
    }

    private void InitializeData()
    {
        foreach (UnitType unitType in Enum.GetValues(typeof(UnitType)))
        {
            _units.Add(unitType, new Queue<Unit>());
            _actions.Add(unitType, new Queue<IUnitAction>());
        }
    }

    private bool HasActionsForUnitType(UnitType unitType)
    {
        return _actions[unitType].Count != 0;
    }
}