using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public UnitHealth UnitHealth { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    // Used for outside scripts
    public delegate void OnUnitDead(Unit unit);

    public event OnUnitDead OnUnitDeath;

    public delegate void OnDamageTaken();

    public OnDamageTaken OnDamageTake;

    [SerializeField] private UnitCombat unitCombat;

    // Used for this specific unit to manage components
    [SerializeField] private UnityEvent onUnitDeath;

    [SerializeField] private UnitSO unitSo;

    public virtual void OnDeath()
    {
        Animator.SetTrigger(Constants.AnimDeathParam);
        onUnitDeath.Invoke();
        OnUnitDeath?.Invoke(this);
    }

    // Used for an event by the attacking animation
    protected void DealDamage()
    {
        unitCombat.DealDamageToTargets();
    }

    private void Awake()
    {
        UnitHealth = new UnitHealth(unitSo.health, unitSo.health);
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }
}