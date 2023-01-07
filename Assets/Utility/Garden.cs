using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Garden : MonoBehaviour, IDamageable, IMultipleUnitAction
{
    public ObjectHealth ObjectHealth { get; set; }
    public event Action<IMultipleUnitAction> OnUnitActionRequired;
    public event Action OnUnitActionFinished;
    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }
    [field: SerializeField] public UnityEvent OnObjRespawn { get; set; }

    [SerializeField] private UnitActionManager unitActionManager;
    [SerializeField] private GardenSO gardenSo;
    [SerializeField] private List<GameObject> gardenStages;

    [SerializeField] private Farm farm;

    private int _currentGardenStage = 0;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(gardenSo.Health, gardenSo.Health);
        farm.AddGarden(this);
    }

    private void Start()
    {
        StartCoroutine(ActionLoop());
        OnUnitActionRequired += ActionManager.Instance.AssignUnitToAction;
    }

    #endregion

    #region IDamageable

    public void OnDamageTake()
    {
        OnDamageTaken?.Invoke();
    }

    public void OnDead()
    {
        OnObjDeath.Invoke();
        OnDeath?.Invoke(this);
        ResetAction();
        ActionManager.Instance.RemoveAction(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetAttackPoint()
    {
        return transform.position;
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

    private bool HasReachedHarvestStage()
    {
        return _currentGardenStage == gardenStages.Count - 1;
    }

    private void ChangeStage()
    {
        gardenStages[_currentGardenStage].SetActive(false);
        ChangeStageIndex();
        gardenStages[_currentGardenStage].SetActive(true);
        StartCoroutine(ActionLoop());
    }

    private void ChangeStageIndex()
    {
        if (HasReachedHarvestStage())
        {
            _currentGardenStage = 0;
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
}