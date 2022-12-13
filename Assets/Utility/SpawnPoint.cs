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
        UnitsToSpawn = new Queue<GameObject>();

        _enemySpawner = GetComponentInParent<EnemySpawner>();
        _enemySpawner.OnWaveGenerated += SpawnUnit;
    }

    private void SpawnUnit()
    {
        if (UnitsToSpawn.Count == 0)
        {
            return;
        }

        GameObject unitGo = UnitsToSpawn.Dequeue();
        Unit unit = unitGo.GetComponent<Unit>(); // TODO: optimize

        unitGo.transform.position = transform.position;
        unitGo.SetActive(true);
        unit.InitUnit();
    }

    private void OnTriggerExit(Collider other)
    {
        SpawnUnit();
    }
}