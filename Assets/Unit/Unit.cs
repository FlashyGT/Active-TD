using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    // Used for this specific unit to manage components
    [SerializeField] private UnityEvent onUnitDeath;

    [SerializeField] private UnitSO unitSo;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(unitSo.health, unitSo.health);
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        Movement.InitMovement();
    }

    #endregion

    #region IDamageable

    public void OnDamageTake()
    {
        OnDamageTaken?.Invoke();
    }

    public void OnDead()
    {
        Animator.SetTrigger(Constants.AnimDeathParam);
        onUnitDeath.Invoke();
        OnDeath?.Invoke(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #endregion

    // Used for an event by the attacking animation
    protected void DealDamage()
    {
        Combat.DealDamageToTargets();
    }
}