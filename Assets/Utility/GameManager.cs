using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action<bool> WaveState;
    
    public event Action GameStarted;
    
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }

    public int FoodAmount {
        get
        {
            return foodAmount;
        }
        set
        {
            foodAmount = value;
            foodAmountUI.text = foodAmount.ToString();
        }
    }
    public int MoneyAmount {
        get
        {
            return moneyAmount;
        }
        set
        {
            moneyAmount = value;
            moneyAmountUI.text = moneyAmount.ToString();
        }
    }
    [SerializeField] private int foodAmount;
    [SerializeField] private int moneyAmount;
    [SerializeField] private TextMeshProUGUI foodAmountUI;
    [SerializeField] private TextMeshProUGUI moneyAmountUI;

    [SerializeField] private GameObject gameLostScreen;
    
    [SerializeField] private EnemySpawner _enemySpawner;

    private Coroutine _gameLostCoroutine;
    
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

    private void Start()
    {
        _enemySpawner.OnWaveGenerated += UpdateWaveState;
        _enemySpawner.OnWaveDefeated += UpdateWaveState;
    }

    #endregion

    #region Combat

    public void DamageObject(IDamageable obj, int damage)
    {
        ObjectHealth objectHealth = obj.ObjectHealth;

        objectHealth.Health -= damage;

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

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void StartGame()
    {
        GameStarted?.Invoke();
        UnpauseGame();

        if (_gameLostCoroutine != null)
        {
            StopCoroutine(_gameLostCoroutine);   
        }
    }

    public void GameLost()
    {
        _gameLostCoroutine = StartCoroutine(PauseGameWithDelay());
        gameLostScreen.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public bool HasMoney()
    {
        return MoneyAmount != 0;
    }

    private IEnumerator PauseGameWithDelay()
    {
        yield return new WaitForSeconds(3f);
        PauseGame();
    }

    private void UpdateWaveState()
    {
        WaveState?.Invoke(_enemySpawner.WaveInProgress);
    }
}