using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Queue<GameObject> UnitsToSpawn { get; private set; }

    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = GetComponentInParent<EnemySpawner>();
    }

    private void Start()
    {
        UnitsToSpawn = new Queue<GameObject>();
        _enemySpawner.OnWaveGenerated += SpawnUnit;
    }

    private void SpawnUnit()
    {
        if (UnitsToSpawn.Count == 0)
        {
            return;
        }

        GameObject unit = UnitsToSpawn.Dequeue();

        unit.transform.position = transform.position;
        unit.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        SpawnUnit();
    }
}