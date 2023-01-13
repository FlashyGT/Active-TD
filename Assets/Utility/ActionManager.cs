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

    private readonly Dictionary<UnitType, Queue<Unit>> _units = new();
    private readonly Dictionary<UnitType, Queue<IMultipleUnitAction>> _actions = new();

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

    private void Start()
    {
        GameManager.Instance.GameStarted += Reset;
    }

    #endregion

    public Vector3 GetEnemyActionDestination(Unit unit)
    {
        DamageableBuilding target;

        if (barricade.buildingBuilt && !barricade.ObjectHealth.IsDead())
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

        target.OnObjDeath.AddListener(unit.Movement.RestartMovement);
        
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

    public void AssignUnitToAction(IMultipleUnitAction unitAction)
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

    public void RemoveAction(IMultipleUnitAction action)
    {
        UnitType type = action.GetUnitType();
        _actions[type] = GameManager.Instance.RemoveItemFromQueue(action, _actions[type]);
    }

    private void InitializeData()
    {
        foreach (UnitType unitType in Enum.GetValues(typeof(UnitType)))
        {
            _units.Add(unitType, new Queue<Unit>());
            _actions.Add(unitType, new Queue<IMultipleUnitAction>());
        }
    }

    private bool HasActionsForUnitType(UnitType unitType)
    {
        return _actions[unitType].Count != 0;
    }

    private void Reset()
    {
        foreach (var unitQueue in _units.Values)
        {
            unitQueue.Clear();
        }
        
        foreach (var actionQueue in _actions.Values)
        {
            actionQueue.Clear();
        }
    }
}