using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    public WeaponType weaponType;

    // Generic Combat
    public int damage;
    public float range; // Combat collider radius

    // Ranged combat
    public float fireRate;
    public GameObject projectile;
}

public enum WeaponType
{
    Melee,
    Ranged
}