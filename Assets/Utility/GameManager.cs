using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }

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

    public void DamageUnit(Unit unit, int damage)
    {
        UnitHealth unitHealth = unit.UnitHealth;

        unitHealth.Health -= damage;
        unit.OnDamageTake?.Invoke();

        if (unitHealth.Health <= 0)
        {
            unit.OnDeath();
        }
    }

    public void HealUnit(Unit unit, int heal)
    {
        UnitHealth unitHealth = unit.UnitHealth;

        unitHealth.Health += heal;

        if (unitHealth.Health > unitHealth.MaxHealth)
        {
            unitHealth.Health = unitHealth.MaxHealth;
        }
    }
}