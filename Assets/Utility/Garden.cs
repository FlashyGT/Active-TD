using System;
using UnityEngine;
using UnityEngine.Events;

public class Garden : MonoBehaviour, IDamageable
{
    public ObjectHealth ObjectHealth { get; set; }

    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    // Used for this specific unit to manage components and callbacks for external scripts
    public UnityEvent onGardenDeath;

    [SerializeField] private GardenSO gardenSo;

    #region UnityMethods

    private void Awake()
    {
        ObjectHealth = new ObjectHealth(gardenSo.health, gardenSo.health);
    }

    #endregion

    #region IDamageable

    public void OnDamageTake()
    {
        OnDamageTaken?.Invoke();
    }

    public void OnDead()
    {
        onGardenDeath.Invoke();
        OnDeath?.Invoke(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetAttackPoint()
    {
        return transform.position;
    }

    #endregion
}