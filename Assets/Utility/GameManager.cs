using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }

    
    public int FoodAmount {
        get
        {
            return _foodAmount;
        }
        set
        {
            _foodAmount = value;
            foodAmountUI.text = _foodAmount.ToString();
        }
    }
    public int MoneyAmount {
        get
        {
            return _moneyAmount;
        }
        set
        {
            _moneyAmount = value;
            moneyAmountUI.text = _moneyAmount.ToString();
        }
    }
    private int _foodAmount;
    private int _moneyAmount;
    [SerializeField] private TextMeshProUGUI foodAmountUI;
    [SerializeField] private TextMeshProUGUI moneyAmountUI;

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

    #region Utility

    public Queue<T> RemoveItemFromQueue<T>(T item, Queue<T> queue)
    {
        return new Queue<T>(queue.Where(x => !Equals(x, item)));
    }

    #endregion

    public void GameLost()
    {
        Time.timeScale = 0;
    }
}