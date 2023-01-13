using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour, ISpawnable
{
    public bool buildingBuilt = false;
    
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }
    [field: SerializeField] public UnityEvent OnObjRespawn { get; set; }

    // Locations and unit count have to match
    [SerializeField] protected List<Vector3> unitLocations;
    [SerializeField] protected List<Unit> units;

    private int _currentUnit = 0;

    #region UnityMethods

    protected virtual void Start()
    {
        
    }

    #endregion

    public virtual Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        throw new NotImplementedException();
    }

    public virtual void Buff()
    {
        throw new NotImplementedException();
    }
    
    public virtual void Build()
    {
        OnObjRespawn?.Invoke();
        buildingBuilt = true;
    }
    
    public virtual void EnableUnit()
    {
        if (_currentUnit == units.Count)
        {
            return;
        }

        if (!buildingBuilt)
        {
            Build();
        }
        
        
        Unit unit = units[_currentUnit];
        Transform unitParent = unit.transform.parent;
            
        unit.Building = this;

        unitParent.position = unitLocations[_currentUnit];
        unitParent.gameObject.SetActive(true);

        _currentUnit++;
    }
}
