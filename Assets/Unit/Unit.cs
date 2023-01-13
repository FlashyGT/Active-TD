using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable, ISpawnable
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; protected set; }

    [field: SerializeField] public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }
    [field: SerializeField] public UnitAction Action { get; private set; }
    [field: SerializeField] public UnitCollisionBlocker UnitCollisionBlocker { get; private set; }

    public Building Building { get; set; }
    public UnitType Type { get; private set; }
    public ObjectHealth ObjectHealth { get; set; }
    public event Action<IDamageable> OnDeath;
    [field: SerializeField] public UnityEvent OnObjDeath { get; set; }
    [field: SerializeField] public UnityEvent OnObjRespawn { get; set; }

    public GameObject Parent { get; private set; }

    [SerializeField] protected UnitSO unitSo;

    #region UnityMethods

    protected virtual void Awake()
    {
        Parent = transform.parent.gameObject;
        InitSO();
        InitUnit();
    }

    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        OnObjRespawn.AddListener(ObjectHealth.ResetHealth);
        GameManager.Instance.GameStarted += Reset;
    }

    #endregion

    #region IDamageable

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
    
    protected virtual void Reset()
    {
        OnObjRespawn.Invoke();
    }

    private IEnumerator InitializeUnit()
    {
        // Waiting for components to get loaded...
        yield return new WaitUntil(ComponentsLoaded);
        Reset();
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