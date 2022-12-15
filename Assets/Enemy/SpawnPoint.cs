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
        _enemySpawner.OnWaveGenerated += StartSpawning;
    }

    private void StartSpawning()
    {
        SpawnUnit();
    }

    private void SpawnUnit()
    {
        if (UnitsToSpawn.Count == 0)
        {
            return;
        }

        GameObject unitGo = UnitsToSpawn.Dequeue();
        Unit unit = unitGo.GetComponentInDirectChildren<Unit>(true); // TODO: optimize

        unitGo.transform.position = transform.position;
        unitGo.SetActive(true);
        unit.OnObjDeath.AddListener(RemoveActiveUnit);
        unit.InitUnit();
    }

    private void RemoveActiveUnit()
    {
        _enemySpawner.AmountOfUnitsInWave--;
    }

    private void OnTriggerExit(Collider other)
    {
        SpawnUnit();
    }
}