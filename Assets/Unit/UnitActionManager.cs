using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionManager : MonoBehaviour
{
    public event Action OnActionFinished;

    [SerializeField] private GameObject actionUIElement;
    [SerializeField] private Image icon;
    [SerializeField] private Image fillerImage;

    [SerializeField] private List<UnitActionSO> unitActions = new();

    private UnitActionSO _currentAction;
    private readonly List<Unit> _unitsInAction = new();
    private float _timeInAction = 0f;

    private UnitActionItem _itemBeforeAction;
    private UnitActionItem _itemAfterAction;
    private bool _onlyOneUnitAction;

    #region UnityMethods

    private void Start()
    {
        _onlyOneUnitAction = unitActions.Count == 1;
        if (_onlyOneUnitAction)
        {
            _currentAction = unitActions[0];
            InitUnitAction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (HasUnitsInAction())
        {
            _timeInAction += Time.deltaTime;
            fillerImage.fillAmount = _timeInAction / _currentAction.SecondsToComplete;

            if (_timeInAction >= _currentAction.SecondsToComplete)
            {
                ResetFiller();
                ActionFinished();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponentInParent<Unit>();
        AddUnitToAction(unit);
    }

    private void OnTriggerExit(Collider other)
    {
        Unit unit = other.GetComponentInParent<Unit>();
        _unitsInAction.Remove(unit);

        if (!HasUnitsInAction())
        {
            ResetFiller();
        }
    }

    #endregion

    public void SetUnitAction(UnitActionType type, UnitActionItem item)
    {
        _currentAction = FindUnitAction(type, item);
        InitUnitAction();
    }

    private UnitActionSO FindUnitAction(UnitActionType type, UnitActionItem item)
    {
        foreach (UnitActionSO unitAction in unitActions)
        {
            if (unitAction.UnitActionType == type && unitAction.UnitActionItem == item)
            {
                return unitAction;
            }
        }

        return null;
    }

    private void InitUnitAction()
    {
        icon.sprite = _currentAction.Icon;

        switch (_currentAction.UnitActionType)
        {
            case UnitActionType.Pickup:
                _itemBeforeAction = UnitActionItem.Empty;
                _itemAfterAction = _currentAction.UnitActionItem;
                break;
            case UnitActionType.Delivery:
                _itemBeforeAction = _currentAction.UnitActionItem;
                _itemAfterAction = UnitActionItem.Empty;
                break;
        }

        actionUIElement.SetActive(true);
    }

    private bool IsUnitValidForAction(Unit unit)
    {
        return unit.Action.Item == _itemBeforeAction;
    }

    private void ActionFinished()
    {
        foreach (Unit unit in _unitsInAction)
        {
            unit.Action.Item = _itemAfterAction;
            unit.Action.OnActionFinished?.Invoke();
        }

        _unitsInAction.Clear();

        if (!_onlyOneUnitAction)
        {
            actionUIElement.SetActive(false);
            OnActionFinished?.Invoke();
        }
    }

    private void AddUnitToAction(Unit unit)
    {
        if (IsUnitValidForAction(unit) && _currentAction != null)
        {
            _unitsInAction.Add(unit);
        }
    }

    private void ResetFiller()
    {
        _timeInAction = 0;
        fillerImage.fillAmount = _timeInAction;
    }

    private bool HasUnitsInAction()
    {
        return _unitsInAction.Count != 0;
    }
}