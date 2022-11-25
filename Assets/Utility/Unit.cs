using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    // Used for outside scripts
    public delegate void OnUnitDead(Unit unit);

    public delegate void OnDamageTaken();

    // Used for this specific unit to manage components
    [SerializeField] private UnityEvent onUnitDeath;

    [SerializeField] private UnitSO unitSo;
    public UnitHealth UnitHealth { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        UnitHealth = new UnitHealth(unitSo.health, unitSo.health);
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
    }

    public event OnUnitDead OnUnitDeath;
    public OnDamageTaken OnDamageTake;

    public virtual void OnDeath()
    {
        Animator.SetTrigger(Constants.AnimDeathParam);
        onUnitDeath.Invoke();
        OnUnitDeath?.Invoke(this);
    }
}