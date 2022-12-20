using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    [field: SerializeField] public UnitActionItem Item { get; set; }
    [field: SerializeField] public UnitType Type { get; private set; }
    public Vector3 CurrentDestination { get; private set; }
    public Action OnActionFinished;

    [SerializeField] private bool isPlayer;

    private Unit _unit;
    private Queue<Vector3> _destinations = new();
    private Vector3 _unitBaseLocation;

    #region UnityMethods

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private void Start()
    {
        if (!isPlayer)
        {
            _unitBaseLocation = _unit.transform.position;
            OnActionFinished += ChangeDestination;
            ActionManager.Instance.UnitIsAvailable(Type, _unit);
        }
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
        ActionManager.Instance.UnitIsAvailable(Type, _unit);
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