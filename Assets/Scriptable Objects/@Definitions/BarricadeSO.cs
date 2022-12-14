using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Barricade", menuName = "Scriptable Objects/Barricade")]
public class BarricadeSO : ScriptableObject
{
    [field: SerializeField] public List<int> HealthPerLevel { get; private set; }
}