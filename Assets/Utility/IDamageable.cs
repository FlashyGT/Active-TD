using System;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }

    // Used by the combat system
    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    // Used for this specific unit to manage components and callbacks for external scripts
    public UnityEvent OnObjDeath { get; set; }

    public void OnDamageTake();

    public void OnDead();

    public GameObject GetGameObject();

    public Vector3 GetAttackPoint();
}