using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Image fillBar;

    // for some reason Unity doesn't allow referencing interfaces, so we use this as a workaround
    [SerializeField] private GameObject obj;
    private IDamageable _objIDamageable;
    private ISpawnable _objISpawnable;
    private ObjectHealth _objHealth;

    private float _health;
    private float _maxHealth;

    #region UnityMethods

    private void Start()
    {
        _objIDamageable = obj.GetComponent<IDamageable>();
        _objISpawnable = obj.GetComponent<ISpawnable>(); // TODO: Refactor
        _objHealth = _objIDamageable.ObjectHealth;
        
        _objISpawnable.OnObjRespawn.AddListener(ResetHealthBar);
        
        _objHealth.OnHealthChange += UpdateHealthBar;
        _objHealth.OnHealthReset += HealthReset;
        HealthReset();
    }

    #endregion

    private void ResetHealthBar()
    {
        container.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (_objHealth.FullHealth())
        {
            container.SetActive(false);
            return;
        }
        
        if (!container.activeInHierarchy && !_objHealth.FullHealth())
        {
            container.SetActive(true);
        }

        _health = _objIDamageable.ObjectHealth.Health;
        ChangeFillAmount();
        ChangeColor();
    }

    private void HealthReset()
    {
        _maxHealth = _objIDamageable.ObjectHealth.MaxHealth;
    }
    
    private void ChangeFillAmount()
    {
        fillBar.fillAmount = _health / _maxHealth;
    }

    private void ChangeColor()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (_health / _maxHealth));
        fillBar.color = healthColor;
    }
}