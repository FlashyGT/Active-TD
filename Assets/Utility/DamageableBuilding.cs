using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableBuilding : Building, IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }
    
    public event Action<IDamageable> OnDeath;

    public virtual void OnDead()
    {
        OnObjDeath.Invoke();
        OnObjDeath.RemoveAllListeners();
        OnDeath?.Invoke(this);
    }

    public virtual GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual Vector3 GetAttackPoint()
    {
        return transform.position;
    }
}
