using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Barricade : DamageableBuilding
{

    [SerializeField] private BarricadeSO barricadeSo;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private int attackPointXDeviation = 5;

    private int _currLevel = 0;

    #region UnityMethods

    private void Awake()
    {
        int health = barricadeSo.HealthPerLevel[_currLevel];
        ObjectHealth = new ObjectHealth(health, health);
    }

    protected override void Start()
    {
        OnObjRespawn.AddListener(ObjectHealth.ResetHealth);
    }

    #endregion

    #region IDamageable

    public override Vector3 GetAttackPoint()
    {
        Vector3 pointPos = attackPoint.position;
        int xDeviation = Random.Range(-attackPointXDeviation, attackPointXDeviation);
        return new Vector3(pointPos.x + xDeviation, 0f, pointPos.z);
    }

    #endregion

    public override void Buff()
    {
        _currLevel++;
        ObjectHealth.MaxHealth = barricadeSo.HealthPerLevel[_currLevel];
        ObjectHealth.ResetHealth();
    }

    public override void Build()
    {
        base.Build();
        Reset();
        GameManager.Instance.GameStarted += Reset;
    }
    
    public override Queue<Vector3> GetActionDestinations(KeyValuePair<UnitActionType, UnitActionItem> action)
    {
        throw new NotImplementedException();
    }

    private void Reset()
    {
        OnObjRespawn?.Invoke();
    }
}