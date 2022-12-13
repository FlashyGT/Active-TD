using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }

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

    #region Combat

    public void DamageObject(IDamageable obj, int damage)
    {
        ObjectHealth objectHealth = obj.ObjectHealth;

        objectHealth.Health -= damage;
        obj.OnDamageTake();

        if (objectHealth.Health <= 0)
        {
            obj.OnDead();
        }
    }

    public void HealObject(IDamageable obj, int heal)
    {
        ObjectHealth objectHealth = obj.ObjectHealth;

        objectHealth.Health += heal;

        if (objectHealth.Health > objectHealth.MaxHealth)
        {
            objectHealth.Health = objectHealth.MaxHealth;
        }
    }

    #endregion

    public void GameLost()
    {
        Time.timeScale = 0;
    }
}