using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] protected Building building;
    
    [SerializeField] protected List<UpgradeSO> upgradeSos = new();

    [Header("Visuals")]
    [SerializeField] private GameObject container;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cost;

    protected int UpgradeLevel = 0;

    [SerializeField] private bool maxUpgradeAtStart = false;

    private UpgradeSO CurrentUpgradeSo => upgradeSos[UpgradeLevel];
    private bool MaxedOut => UpgradeLevel == upgradeSos.Count;
    
    private int _playerMoney;
    private int _upgradeCost; // How much money needs to be spent to upgrade
    private int _spentOnUpgrade; // How much money has been spent so far
    private int _moneyPerTick; // How much money will get removed this frame

    private float _timeInUpgrade;

    private Collider _collider;
    
    #region UnityMethods

    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();

        GameManager.Instance.WaveState += ToggleUpgradeOnWaveState;
        
        if (maxUpgradeAtStart)
        {
            MaxUpgrades();
        }
        else
        {
            SetUpgrade(upgradeSos[UpgradeLevel]);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        _timeInUpgrade += Time.deltaTime;
        
        if (_timeInUpgrade < 1f || !GameManager.Instance.HasMoney())
        {
            return;
        }

        _playerMoney = GameManager.Instance.MoneyAmount;
        _moneyPerTick = 2 * (int)_timeInUpgrade;

        if (_moneyPerTick > _playerMoney)
        {
            _moneyPerTick = _playerMoney;
        }
        
        GameManager.Instance.MoneyAmount -= _moneyPerTick;
        
        _spentOnUpgrade += _moneyPerTick;
        cost.text = (_upgradeCost - _spentOnUpgrade).ToString();

        if (_spentOnUpgrade == _upgradeCost)
        {
            NextUpgrade();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _timeInUpgrade = 0f;
    }

    #endregion

    private void MaxUpgrades()
    {
        for (UpgradeLevel = 0; UpgradeLevel < upgradeSos.Count; UpgradeLevel++)
        {
            Upgrade();
        }

        TurnOffUpgrade();
    }

    private void NextUpgrade()
    {
        Upgrade();
        SwitchUpgrade();
    }
    
    private void Upgrade()
    {
        switch (CurrentUpgradeSo.UpgradeType)
        {
            case UpgradeType.Unit:
                building.EnableUnit();
                break;
            case UpgradeType.Building:
                building.Build();
                break;
            case UpgradeType.BuildingBuff:
                building.Buff();
                break;
        }
    }

    private void ResetUpgrade()
    {
        _timeInUpgrade = 0f;
        _spentOnUpgrade = 0;
    }
    
    private void SwitchUpgrade()
    {
        ResetUpgrade();
        UpgradeLevel++;
        
        if (MaxedOut)
        {
            TurnOffUpgrade();
            return;
        }
        
        SetUpgrade(upgradeSos[UpgradeLevel]);
    }
    
    private void SetUpgrade(UpgradeSO upgradeSo)
    {
        icon.sprite = upgradeSo.Icon;
        description.text = upgradeSo.Description;
        cost.text = upgradeSo.Cost.ToString();
        
        _upgradeCost = upgradeSo.Cost;
        
        container.SetActive(true);
    }

    private void TurnOffUpgrade()
    {
        container.SetActive(false);
        _collider.enabled = false;   
    }
    
    private void ToggleUpgradeOnWaveState(bool canUpdate)
    {
        if (MaxedOut)
        {
            return;
        }
        
        container.SetActive(!canUpdate);
        _collider.enabled = !canUpdate;       
    }
}
