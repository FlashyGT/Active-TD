using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEvents : MonoBehaviour
{
    private Unit _unit;

    #region UnityMethods

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    #endregion

    protected void DealDamage()
    {
        _unit.Combat.DealDamageToTargets();
    }
}