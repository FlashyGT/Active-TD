using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionManager : MonoBehaviour
{
    public event Action OnActionFinished;

    [SerializeField] private UAMType uamType;
    
    [SerializeField] private GameObject actionGroundElement;
    [SerializeField] private GameObject actionUIElement;
    [SerializeField] private Image icon;
    [SerializeField] private Image fillerImage;

    [SerializeField] private List<UnitActionSO> definedActions = new();

    private Queue<KeyValuePair<UnitActionSO, Action>> _actionQueue = new();
    
    private KeyValuePair<UnitActionSO, Action> _currentAction;
    private UnitActionSO CurrentActionSo => _currentAction.Key;
    private Action CurrentActionCallback => _currentAction.Value;

    private readonly List<Unit> _unitsInAction = new();
    private float _timeInAction = 0f;

    private UnitActionItem _itemBeforeAction;
    private UnitActionItem _itemAfterAction;
    private bool _onlyOneUnitAction;

    #region UnityMethods

    private void Start()
    {
        OnActionFinished += StartNextActionInQueue;

        _onlyOneUnitAction = definedActions.Count == 1;
        if (_onlyOneUnitAction)
        {
            _currentAction = new KeyValuePair<UnitActionSO, Action>(definedActions[0], null);
            InitUnitAction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (HasUnitsInAction() && IsActionDoable())
        {
            _timeInAction += Time.deltaTime;
            fillerImage.fillAmount = _timeInAction / CurrentActionSo.SecondsToComplete;

            if (_timeInAction >= CurrentActionSo.SecondsToComplete)
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

    public KeyValuePair<UnitActionType, UnitActionItem> GetCurrentAction()
    {
        if (CurrentActionSo == null)
        {
            return default;
        }
        
        UnitActionSO currAction = CurrentActionSo;
        return new KeyValuePair<UnitActionType, UnitActionItem>(currAction.UnitActionType, currAction.UnitActionItem);
    }
    
    public void SetUnitAction(KeyValuePair<UnitActionType, UnitActionItem> action, Action callback)
    {
        UnitActionSO actionSO = FindUnitAction(action);
        var currAction = new KeyValuePair<UnitActionSO, Action>(actionSO, callback);

        if (CurrentActionSo == null)
        {
            _currentAction = currAction;
            InitUnitAction();
        }
        else
        {
            _actionQueue.Enqueue(currAction);
        }
    }

    private void StartNextActionInQueue()
    {
        if (_actionQueue.Count != 0)
        {
            _currentAction = _actionQueue.Dequeue();
            InitUnitAction();
        }
    }
    
    private UnitActionSO FindUnitAction(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        foreach (UnitActionSO unitAction in definedActions)
        {
            if (unitAction.UnitActionType == action.Key && unitAction.UnitActionItem == action.Value)
            {
                return unitAction;
            }
        }

        return null;
    }

    private void InitUnitAction()
    {
        icon.sprite = CurrentActionSo.Icon;

        switch (CurrentActionSo.UnitActionType)
        {
            case UnitActionType.Pickup:
                _itemBeforeAction = UnitActionItem.Empty;
                _itemAfterAction = CurrentActionSo.UnitActionItem;
                break;
            case UnitActionType.Delivery:
                _itemBeforeAction = CurrentActionSo.UnitActionItem;
                _itemAfterAction = UnitActionItem.Empty;
                break;
        }

        ToggleActionElements();
    }

    private void ToggleActionElements()
    {
        actionGroundElement.SetActive(!actionGroundElement.activeInHierarchy);
        actionUIElement.SetActive(!actionUIElement.activeInHierarchy);
    }

    private bool IsUnitValidForAction(Unit unit)
    {
        return unit.Action.Item == _itemBeforeAction &&
               (unit.Type == CurrentActionSo.UnitType || unit.Type == UnitType.Universal);
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
            CurrentActionCallback?.Invoke();
            _currentAction = default;
            
            ToggleActionElements();
            OnActionFinished?.Invoke();
        }
        else
        {
            if (uamType == UAMType.Food)
            {
                GameManager.Instance.FoodAmount--;
            }
        }
    }

    private void AddUnitToAction(Unit unit)
    {
        if (CurrentActionSo != null && IsUnitValidForAction(unit))
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

    private bool IsActionDoable()
    {
        if (uamType == UAMType.Food)
        {
            return GameManager.Instance.FoodAmount != 0;
        }

        return true;
    }
}

public enum UAMType
{
    Universal,
    Food
}