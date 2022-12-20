using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Garden", menuName = "Scriptable Objects/Garden")]
public class GardenSO : ScriptableObject
{
    public int health;
    public float timeBetweenStages;
}