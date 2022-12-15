using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Garden : MonoBehaviour, IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }

    [SerializeField] private GardenSO gardenSo;
    [SerializeField] private List<GameObject> gardenStages;
    [SerializeField] private float timeBetweenStages = 5f;

    private UnitActionManager _unitActionManager;
    private int _currentGardenStage = 0;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(gardenSo.health, gardenSo.health);
        _unitActionManager = GetComponent<UnitActionManager>();
    }

    private void Start()
    {
        StartCoroutine(ActionLoop());
        _unitActionManager.OnActionFinished += ChangeStage;
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

    private void ChangeStage()
    {
        gardenStages[_currentGardenStage].SetActive(false);
        ChangeStageIndex();
        gardenStages[_currentGardenStage].SetActive(true);
        StartCoroutine(ActionLoop());
    }

    private void ChangeStageIndex()
    {
        if (ReachedFinalStage())
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
        if (ReachedFinalStage())
        {
            _unitActionManager.SetUnitAction(UnitActionType.Pickup, UnitActionItem.Empty);
        }
        else
        {
            _unitActionManager.SetUnitAction(UnitActionType.Delivery, UnitActionItem.Water);
        }
    }

    private bool ReachedFinalStage()
    {
        return _currentGardenStage == gardenStages.Count - 1;
    }

    private IEnumerator ActionLoop()
    {
        yield return new WaitForSeconds(timeBetweenStages);
        StartAction();
    }
}