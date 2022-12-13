using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    public List<IDamageable> Targets { get; private set; }

    protected Unit Unit;

    [SerializeField] private WeaponSO weaponSo;

    #region UnityMethods

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
        Targets = new List<IDamageable>();
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

    public void DealDamageToTargets()
    {
        // ToList() copies the existing list, so when damage gets dealt and a unit dies
        // we don't modify the actual _targets list, but instead a copy of it.
        // If we modify _targets directly we receive: InvalidOperationException
        foreach (IDamageable obj in Targets.ToList())
        {
            GameManager.Instance.DamageObject(obj, weaponSo.damage);
        }
    }

    private void AddTarget(IDamageable obj)
    {
        Targets.Add(obj);
        obj.OnDeath += RemoveTarget;

        if (Targets.Count == 1)
        {
            OnCombatStarted?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, true);
        }
    }

    private void RemoveTarget(IDamageable obj)
    {
        Targets.Remove(obj);

        if (Targets.Count == 0)
        {
            OnCombatEnded?.Invoke();
            Unit.Animator.SetBool(Constants.AnimAttackParam, false);
        }
    }
}