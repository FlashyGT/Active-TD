using System;
using UnityEngine;

public interface IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    public void OnDamageTake();

    public void OnDead();

    public GameObject GetGameObject();

    public Vector3 GetAttackPoint();
}