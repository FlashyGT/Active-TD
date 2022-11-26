using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;

    private Animator _animator;
    private List<Unit> _targets = new();

    public void DealDamageToTargets()
    {
        // ToList() copies the existing list, so when damage gets dealt and a unit dies
        // we don't modify the actual _targets list, but instead a copy of it.
        // If we modify _targets directly we receive: InvalidOperationException
        foreach (Unit unit in _targets.ToList())
        {
            GameManager.Instance.DamageUnit(unit, weaponSo.damage);
        }
    }

    private void Start()
    {
        _animator = GetComponentInParent<Unit>().Animator;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Unit unit = collider.GetComponentInParent<Unit>();
        AddTarget(unit);
        _animator.SetBool(Constants.AnimAttackParam, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        Unit unit = collider.GetComponentInParent<Unit>();
        RemoveTarget(unit);
        _animator.SetBool(Constants.AnimAttackParam, false);
    }

    private void AddTarget(Unit unit)
    {
        _targets.Add(unit);
        unit.OnUnitDeath += RemoveTarget;
    }

    private void RemoveTarget(Unit unit)
    {
        _targets.Remove(unit);
        if (_targets.Count == 0)
        {
            _animator.SetBool(Constants.AnimAttackParam, false);
        }
    }
}