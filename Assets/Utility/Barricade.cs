using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Barricade : MonoBehaviour, IDamageable, IUpgradeable
{
    public ObjectHealth ObjectHealth { get; set; }

    // Used by the combat system
    public event Action<IDamageable> OnDeath;
    public event Action OnDamageTaken;

    // Used for this specific object to manage components and callbacks for external scripts
    public UnityEvent onObjDeath;

    [SerializeField] private BarricadeSO barricadeSo;

    [SerializeField] private Transform attackPoint;
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
        onObjDeath.Invoke();
        OnDeath?.Invoke(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #endregion

    public Vector3 GetAttackPoint()
    {
        Vector3 pointPos = attackPoint.position;
        float xDeviation = Random.Range(-attackPointXDeviation, attackPointXDeviation);
        return new Vector3(pointPos.x + xDeviation, 0f, pointPos.z);
    }

    public void Upgrade()
    {
        throw new NotImplementedException();
    }
}