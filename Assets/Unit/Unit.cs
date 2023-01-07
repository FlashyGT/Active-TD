using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; protected set; }

    [field: SerializeField] public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }
    [field: SerializeField] public UnitAction Action { get; private set; }
    [field: SerializeField] public UnitCollisionBlocker UnitCollisionBlocker { get; private set; }

    public UnitBuilding UnitBuilding { get; set; }
    public UnitType Type { get; private set; }
    public ObjectHealth ObjectHealth { get; set; }
    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }
    [field: SerializeField] public UnityEvent OnObjRespawn { get; set; }

    [SerializeField] protected UnitSO unitSo;

    #region UnityMethods

    protected virtual void Awake()
    {
        InitSO();
        InitUnit();
    }

    protected virtual void Start()
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
        Vector3 attackPoint = transform.position + Vector3.up;

        // Moving
        if (Rigidbody.velocity != Vector3.zero)
        {
            attackPoint += Rigidbody.velocity;
        }

        return attackPoint;
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
        HasFinishedLoading = true;
    }

    private bool ComponentsLoaded()
    {
        bool movement = Movement == null || Movement.HasFinishedLoading;
        bool combat = Combat == null || Combat.HasFinishedLoading;
        bool action = Action == null || Action.HasFinishedLoading;
        bool unitCollisionBlocker = UnitCollisionBlocker == null || UnitCollisionBlocker.HasFinishedLoading;
        return movement && combat && action && unitCollisionBlocker;
    }

    private void InitSO()
    {
        Type = unitSo.UnitType;
        ObjectHealth = new ObjectHealth(unitSo.Health, unitSo.Health);
    }
}