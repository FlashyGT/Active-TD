using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }
    [field: SerializeField] public UnitAction Action { get; private set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }
    [field: SerializeField] public UnityEvent OnObjRespawn { get; set; }

    [SerializeField] private UnitSO unitSo;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(unitSo.health, unitSo.health);
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        OnObjRespawn.AddListener(ObjectHealth.ResetHealth);
    }

    #endregion

    #region IDamageable

    public void OnDamageTake()
    {
        OnDamageTaken?.Invoke();
    }

    public virtual void OnDead()
    {
        Animator.SetTrigger(Constants.AnimDeathParam);
        OnObjDeath.Invoke();
        OnObjDeath.RemoveAllListeners();
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

    public void InitUnit()
    {
        StartCoroutine(InitializeUnit());
    }

    private IEnumerator InitializeUnit()
    {
        // Waiting for components to get loaded...
        yield return new WaitUntil(ComponentsLoaded);
        OnObjRespawn.Invoke();
    }

    private bool ComponentsLoaded()
    {
        return Movement.HasFinishedLoading && Combat.HasFinishedLoading;
    }
}