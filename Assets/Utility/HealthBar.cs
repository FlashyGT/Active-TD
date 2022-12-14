using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Image fillBar;

    // for some reason Unity doesn't allow referencing interfaces, so we use this as a workaround
    [SerializeField] private GameObject objWithIDamageable;
    private IDamageable _obj;

    private float _health;
    private float _maxHealth;

    #region UnityMethods

    private void Start()
    {
        _obj = objWithIDamageable.GetComponent<IDamageable>();
        _obj.OnDamageTaken += UpdateHealthBar;
        _maxHealth = _obj.ObjectHealth.MaxHealth;
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

        _health = _obj.ObjectHealth.Health;
        ChangeFillAmount();
        ChangeColor();
    }
}