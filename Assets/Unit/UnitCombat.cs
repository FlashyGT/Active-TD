using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public List<IDamageable> Targets { get; private set; }

    [SerializeField] private WeaponSO weaponSo;

    private Unit _unit;

    #region UnityMethods

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
        Targets = new List<IDamageable>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        IDamageable obj = coll.GetComponentInParent<IDamageable>();
        AddTarget(obj);
        _unit.Animator.SetBool(Constants.AnimAttackParam, true);
    }

    private void OnTriggerExit(Collider coll)
    {
        IDamageable obj = coll.GetComponentInParent<IDamageable>();
        RemoveTarget(obj);
        _unit.Animator.SetBool(Constants.AnimAttackParam, false);
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
    }

    private void RemoveTarget(IDamageable obj)
    {
        Targets.Remove(obj);
        if (Targets.Count == 0)
        {
            _unit.Animator.SetBool(Constants.AnimAttackParam, false);
        }
    }
}