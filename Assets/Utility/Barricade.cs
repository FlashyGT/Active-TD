using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IDamageable, IUpgradeable
{
    public ObjectHealth ObjectHealth { get; set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    [SerializeField] private BarricadeSO barricadeSo;

    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float attackPointXDeviation = 5f;

    private int _currLevel = 0;

    #region UnityMethods

    private void Awake()
    {
        int health = barricadeSo.healthPerLevel[_currLevel];
        ObjectHealth = new ObjectHealth(health, health);
    }

    #endregion

    #region IDamageable

    public void OnDamageTake()
    {
        OnDamageTaken?.Invoke();
    }

    public void OnDead()
    {
        OnDeath?.Invoke(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #endregion

    public void Upgrade()
    {
        throw new NotImplementedException();
    }
}