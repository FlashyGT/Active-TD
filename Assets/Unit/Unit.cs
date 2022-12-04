using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public UnitHealth UnitHealth { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }

    public event Action<Unit> OnUnitDeath;
    public Action OnDamageTaken;

    // Used for this specific unit to manage components
    [SerializeField] private UnityEvent onUnitDeath;

    [SerializeField] private UnitSO unitSo;

    #region UnityMethods

    private void Awake()
    {
        UnitHealth = new UnitHealth(unitSo.health, unitSo.health);
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    #endregion

    public virtual void OnDeath()
    {
        Animator.SetTrigger(Constants.AnimDeathParam);
        onUnitDeath.Invoke();
        OnUnitDeath?.Invoke(this);
    }

    // Used for an event by the attacking animation
    protected void DealDamage()
    {
        Combat.DealDamageToTargets();
    }
}