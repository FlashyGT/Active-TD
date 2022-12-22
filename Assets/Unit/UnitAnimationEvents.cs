using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEvents : MonoBehaviour
{
    private Unit _unit;

    #region UnityMethods

    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
    }

    #endregion

    protected void DealDamage()
    {
        _unit.Combat.DealDamageToTargets();
    }

    protected void UnitDead()
    {
        _unit.transform.parent.gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}