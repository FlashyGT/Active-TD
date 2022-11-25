using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;

    private void OnTriggerEnter(Collider collider)
    {
        var unit = collider.GetComponentInParent<Unit>();
        Debug.Log(unit.gameObject.name + " starting health: " + unit.UnitHealth.Health);
        GameManager.Instance.DamageUnit(unit, weaponSo.damage);
        Debug.Log(unit.gameObject.name + " current health: " + unit.UnitHealth.Health);
    }
}