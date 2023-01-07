using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Objects/Unit")]
public class UnitSO : ScriptableObject
{
    [field: SerializeField] public UnitType UnitType { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
}