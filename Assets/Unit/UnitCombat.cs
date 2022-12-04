using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public List<Unit> Targets { get; private set; }

    [SerializeField] private WeaponSO weaponSo;

    private Unit _unit;

    #region UnityMethods

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
        Targets = new List<Unit>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        Unit unit = coll.GetComponentInParent<Unit>();
        AddTarget(unit);
        _unit.Animator.SetBool(Constants.AnimAttackParam, true);
    }

    private void OnTriggerExit(Collider coll)
    {
        Unit unit = coll.GetComponentInParent<Unit>();
        RemoveTarget(unit);
        _unit.Animator.SetBool(Constants.AnimAttackParam, false);
    }

    #endregion

    public void DealDamageToTargets()
    {
        // ToList() copies the existing list, so when damage gets dealt and a unit dies
        // we don't modify the actual _targets list, but instead a copy of it.
        // If we modify _targets directly we receive: InvalidOperationException
        foreach (Unit unit in Targets.ToList())
        {
            GameManager.Instance.DamageUnit(unit, weaponSo.damage);
        }
    }

    private void AddTarget(Unit unit)
    {
        Targets.Add(unit);
        unit.OnUnitDeath += RemoveTarget;
    }

    private void RemoveTarget(Unit unit)
    {
        Targets.Remove(unit);
        if (Targets.Count == 0)
        {
            _unit.Animator.SetBool(Constants.AnimAttackParam, false);
        }
    }
}