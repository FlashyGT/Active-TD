using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Image fillBar;
    [SerializeField] private Unit unit;

    private float _health;
    private float _maxHealth;

    #region UnityMethods

    private void Start()
    {
        unit.OnDamageTaken += UpdateHealthBar;
        _maxHealth = unit.ObjectHealth.MaxHealth;
    }

    private void Update()
    {
        transform.LookAt(GameManager.Instance.MainCamera.transform);
    }

    #endregion

    private void ChangeFillAmount()
    {
        fillBar.fillAmount = _health / _maxHealth;
    }

    private void ChangeColor()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (_health / _maxHealth));
        fillBar.color = healthColor;
    }

    private void UpdateHealthBar()
    {
        if (!container.activeInHierarchy)
        {
            container.SetActive(true);
        }

        _health = unit.ObjectHealth.Health;
        ChangeFillAmount();
        ChangeColor();
    }
}