using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Garden", menuName = "Scriptable Objects/Garden")]
public class GardenSO : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float TimeBetweenStages { get; private set; }
}