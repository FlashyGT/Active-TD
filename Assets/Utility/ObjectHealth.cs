using System;

public class ObjectHealth
{
    public event Action OnHealthChange;
    public event Action OnHealthReset;
    
    public int Health {
        get => _health;
        set
        {
            _health = value;
            OnHealthChange?.Invoke();
        }
    }

    public int MaxHealth { get; set; }

    private int _health;
    
    public ObjectHealth(int currentHealth, int maxHealth)
    {
        Health = currentHealth;
        MaxHealth = maxHealth;
    }

    public void ResetHealth()
    {
        Health = MaxHealth;
        OnHealthReset?.Invoke();
    }

    public bool FullHealth()
    {
        return Health == MaxHealth;
    }
    
    public bool IsDead()
    {
        return Health <= 0;
    }
}