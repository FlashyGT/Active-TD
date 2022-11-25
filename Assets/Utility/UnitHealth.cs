using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    public int Health { get; set; }

    public int MaxHealth { get; set; }

    public UnitHealth(int currentHealth, int maxHealth)
    {
        Health = currentHealth;
        MaxHealth = maxHealth;
    }
}