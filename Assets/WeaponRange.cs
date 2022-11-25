using System.Collections.Generic;
using UnityEngine;

public class WeaponRange : MonoBehaviour
{
    private Animator _animator;
    private List<Unit> _targets = new();

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