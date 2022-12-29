using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; private set; }

    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    public int TargetCount => _targets.Count;
    public IDamageable CurrentTarget => _targets[0];

    protected Unit Unit;

    [SerializeField] protected WeaponSO weaponSo;

    private List<IDamageable> _targets;

    #region UnityMethods

    protected virtual void Awake()
    {
        _targets = new List<IDamageable>();
    }

    protected virtual void Start()
    {
        InitSOValues();
        Unit = GetComponentInParent<Unit>();
        Unit.OnObjRespawn.AddListener(_targets.Clear);
        HasFinishedLoading = true;
    }

    private void OnTriggerEnter(Collider coll)
    {
        IDamageable obj = coll.GetComponentInParent<IDamageable>();
        AddTarget(obj);
    }

    private void OnTriggerExit(Collider coll)
    {
        IDamageable obj = coll.GetComponentInParent<IDamageable>();
        RemoveTarget(obj);
    }

    #endregion

    public virtual void DealDamage()
    {
        // ToList() copies the existing list, so when damage gets dealt and a unit dies
        // we don't modify the actual _targets list, but instead a copy of it.
        // If we modify _targets directly we receive: InvalidOperationException
        foreach (IDamageable obj in _targets.ToList())
        {
            GameManager.Instance.DamageObject(obj, weaponSo.damage);
        }
    }

    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    protected void InitSOValues()
    {
        SphereCollider combatCollider = GetComponent<SphereCollider>();
        combatCollider.radius = weaponSo.range;
    }

    private void AddTarget(IDamageable obj)
    {
        _targets.Add(obj);
        obj.OnDeath += RemoveTarget;

        if (TargetCount == 1)
        {
            OnCombatStarted?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, true);
        }
    }

    private void RemoveTarget(IDamageable obj)
    {
        _targets.Remove(obj);

        if (TargetCount == 0)
        {
            OnCombatEnded?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, false);
        }
    }
}