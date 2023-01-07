using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; protected set; }

    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    public int TargetCount => Targets.Count;
    public IDamageable CurrentTarget => GetCurrentTarget();

    protected Unit Unit;

    [SerializeField] protected UnitCombatSO unitCombatSo;

    protected List<IDamageable> Targets;

    #region UnityMethods

    protected virtual void Awake()
    {
        Targets = new List<IDamageable>();
        InitSO();
    }

    protected virtual void Start()
    {
        Unit = GetComponentInParent<Unit>();
        Unit.OnObjRespawn.AddListener(Targets.Clear);
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

    public virtual bool IsUnitInCombat()
    {
        return TargetCount > 0;
    }
    
    public virtual void DealDamage()
    {
        // ToList() copies the existing list, so when damage gets dealt and a unit dies
        // we don't modify the actual _targets list, but instead a copy of it.
        // If we modify _targets directly we receive: InvalidOperationException
        foreach (IDamageable obj in Targets.ToList())
        {
            GameManager.Instance.DamageObject(obj, unitCombatSo.Damage);
        }
    }

    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    protected void InitSO()
    {
        SphereCollider combatCollider = GetComponent<SphereCollider>();
        combatCollider.radius = unitCombatSo.Range;
    }

    public virtual void StopCombat()
    {
        Unit.Animator.SetBool(Constants.AnimAttackParam, false);
    }
    
    public virtual void StartCombat()
    {
        if (IsUnitInCombat())
        {
            Unit.Animator.SetBool(Constants.AnimAttackParam, true);   
        }
    }

    protected virtual bool AllowedToStartCombat()
    {
        return TargetCount == 1;
    }
    
    private void AddTarget(IDamageable obj)
    {
        Targets.Add(obj);
        obj.OnDeath += RemoveTarget;

        if (AllowedToStartCombat())
        {
            OnCombatStarted?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, true);
        }
    }

    private void RemoveTarget(IDamageable obj)
    {
        Targets.Remove(obj);

        if (TargetCount == 0)
        {
            OnCombatEnded?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, false);
        }
    }

    private IDamageable GetCurrentTarget()
    {
        return TargetCount != 0 ? Targets[0] : null;
    }
}