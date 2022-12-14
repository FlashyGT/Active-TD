using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    [field: SerializeField] public UnitActionItem CarryingItem { get; set; }

    public ObjectHealth ObjectHealth { get; set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [field: SerializeField] public UnitCombat Combat { get; private set; }
    [field: SerializeField] public UnitMovement Movement { get; private set; }

    // Used by the combat system
    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    // Used for this specific unit to manage components and callbacks for external scripts
    public UnityEvent onUnitDeath;

    [SerializeField] private UnitSO unitSo;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(unitSo.health, unitSo.health);
        Rigidbody = GetComponent<Rigidbody>();
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
        onUnitDeath.Invoke();
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
        Movement.InitMovement();
    }
}