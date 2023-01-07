using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Combat", menuName = "Scriptable Objects/Unit Combat")]
public class UnitCombatSO : ScriptableObject
{
    [field: SerializeField] public WeaponType WeaponType { get; private set; }

    // Generic Combat
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Range { get; private set; } // Combat collider radius

    // Ranged combat
    [field: SerializeField] public float FireRate { get; private set; }
    [field: SerializeField] public int AttacksBeforeResupply { get; private set; }
    [field: SerializeField] public int FoodRequiredAfter { get; private set; }
    [field: SerializeField] public GameObject Projectile { get; private set; }
}

public enum WeaponType
{
    Melee,
    Ranged
}