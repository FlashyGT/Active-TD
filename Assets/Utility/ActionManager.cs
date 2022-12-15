using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }

    [SerializeField] private Barricade barricade;
    [SerializeField] private Farm farm;

    #region UnityMethods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public Vector3 GetEnemyActionDestination(Unit unit)
    {
        IDamageable target;

        if (!barricade.ObjectHealth.IsDead())
        {
            target = barricade;
        }
        else
        {
            target = farm.GetGarden();
        }

        if (target == null)
        {
            // No target, return units position, so it doesn't move
            return unit.transform.position;
        }

        target.OnObjDeath.AddListener(unit.Movement.InitMovement);
        return target.GetAttackPoint();
    }
}