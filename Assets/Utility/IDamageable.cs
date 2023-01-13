using System;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }

    // Used by the combat system
    public event Action<IDamageable> OnDeath;

    public void OnDead();

    public GameObject GetGameObject();

    public Vector3 GetAttackPoint();
}