using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth
{
    public int Health { get; set; }

    public int MaxHealth { get; set; }

    public ObjectHealth(int currentHealth, int maxHealth)
    {
        Health = currentHealth;
        MaxHealth = maxHealth;
    }

    public void ResetHealth()
    {
        Health = MaxHealth;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }
}