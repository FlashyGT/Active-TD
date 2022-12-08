using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Scriptable Objects/Wave")]
public class WaveSO : ScriptableObject
{
    [field: SerializeField] public List<WaveObject> WaveObjects { get; private set; }
}

[Serializable]
public class WaveObject
{
    [field: SerializeField] public SpawnLocation SpawnLoc { get; private set; }
    [field: SerializeField] public int AmountOfUnits { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}

public enum SpawnLocation
{
    Random, // 0
    LeftSmallTent, // 1
    LeftBigTent, // 2
    MiddleTent, // 3
    RightSmallTent, // 4
    RightBigTent // 5
}