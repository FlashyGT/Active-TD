using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; protected set; }
    
    [field: SerializeField] public UnitActionItem Item { get; set; }
    public Vector3 CurrentDestination { get; private set; }
    public Action OnActionFinished;

    [SerializeField] private bool isPlayer;

    private Unit _unit;
    private UnitType _unitType;
    
    private Queue<Vector3> _destinations = new();
    private Vector3 _unitBaseLocation;

    #region UnityMethods

    private void Start()
    {
        if (!isPlayer)
        {
            _unit = GetComponentInParent<Unit>();
            _unitType = _unit.Type;
            
            _unitBaseLocation = _unit.transform.position;
            OnActionFinished += ChangeDestination;
            ActionManager.Instance.UnitIsAvailable(_unitType, _unit);
        }

        HasFinishedLoading = true;
    }

    #endregion

    public void StartAction(Queue<Vector3> destinations)
    {
        _destinations = destinations;
        ChangeDestination();
    }

    public void ResetAction()
    {
        Item = UnitActionItem.Empty;
        CurrentDestination = _unitBaseLocation;
        ActionManager.Instance.UnitIsAvailable(_unitType, _unit);
        _unit.Movement.InitMovement();
    }

    private void ChangeDestination()
    {
        if (_destinations.Count != 0)
        {
            CurrentDestination = _destinations.Dequeue();
            _unit.Movement.InitMovement();
        }
    }
}