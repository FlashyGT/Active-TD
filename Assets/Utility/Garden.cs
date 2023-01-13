using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Garden : DamageableBuilding, IMultipleUnitAction
{
    public event Action<IMultipleUnitAction> OnUnitActionRequired;
    public event Action OnUnitActionFinished;
    
    [SerializeField] private UnitActionManager unitActionManager;
    [SerializeField] private GardenSO gardenSo;
    [SerializeField] private List<GameObject> gardenStages;

    [SerializeField] private Farm farm;

    private Coroutine _actionCoroutine;
    
    private int _currentGardenStage = 0;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(gardenSo.Health, gardenSo.Health);
    }

    protected override void Start()
    {
        OnUnitActionRequired += ActionManager.Instance.AssignUnitToAction;
        OnObjRespawn.AddListener(ObjectHealth.ResetHealth);
    }

    #endregion

    #region IDamageable

    public override void OnDead()
    {
        base.OnDead();
        ResetAction();
        ActionManager.Instance.RemoveAction(this);
    }

    #endregion

    #region IMultipleUnitAction

    public Queue<Vector3> GetUnitDestinations()
    {
        Queue<Vector3> destinations = new();

        if (!HasReachedHarvestStage())
        {
            destinations.Enqueue(farm.WellLocation);
        }

        destinations.Enqueue(GetUAMLocation());
        return destinations;
    }

    public UnitType GetUnitType()
    {
        return UnitType.Gardener;
    }

    public Vector3 GetUAMLocation()
    {
        return unitActionManager.transform.position;
    }

    #endregion

    public override void Build()
    {
        base.Build();
        Reset();
        GameManager.Instance.GameStarted += Reset;
    }

    public override Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        throw new NotImplementedException();
    }
    
    private bool HasReachedHarvestStage()
    {
        return _currentGardenStage == gardenStages.Count - 1;
    }

    private void ChangeStage()
    {
        gardenStages[_currentGardenStage].SetActive(false);
        ChangeStageIndex();
        gardenStages[_currentGardenStage].SetActive(true);
        _actionCoroutine = StartCoroutine(ActionLoop());
    }

    private void ChangeStageIndex()
    {
        if (HasReachedHarvestStage())
        {
            _currentGardenStage = 0;
            GameManager.Instance.FoodAmount++;
        }
        else
        {
            _currentGardenStage++;
        }
    }

    private void StartAction()
    {
        UnitActionType actionType;
        UnitActionItem actionItem;
        
        if (HasReachedHarvestStage())
        {
            actionType = UnitActionType.Pickup;
            actionItem = UnitActionItem.Empty;
        }
        else
        {
            actionType = UnitActionType.Delivery;
            actionItem = UnitActionItem.Water;
        }
        
        
        var action = new KeyValuePair<UnitActionType, UnitActionItem>(actionType, actionItem);
        Action callback = ChangeStage;
        unitActionManager.SetUnitAction(action, callback);
        
        OnUnitActionRequired?.Invoke(this);
    }

    private IEnumerator ActionLoop()
    {
        ResetAction();
        yield return new WaitForSeconds(gardenSo.TimeBetweenStages);
        StartAction();
    }

    private void ResetAction()
    {
        OnUnitActionFinished?.Invoke();
        OnUnitActionFinished = null;
    }

    private void Reset()
    {
        farm.AddGarden(this);
        OnObjRespawn?.Invoke();

        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
        }

        gardenStages[_currentGardenStage].SetActive(false);
        _currentGardenStage = 0;
        gardenStages[_currentGardenStage].SetActive(true);
        _actionCoroutine = StartCoroutine(ActionLoop());
    }
}