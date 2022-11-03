using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    private int currentHealth;
    private int maxHealth;

    public int Health
    {
        get { return currentHealth; }
        private set { }
    }

    public UnitHealth(int currentHealth, int maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    public bool UnitIsDead()
    {
        return currentHealth < 0;
    }

    public void DamageUnit(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }

    public void HealUnit(int heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
