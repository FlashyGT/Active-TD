using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<WaveSO> waves = new();

    [SerializeField] private List<SpawnPoint> spawnPoints = new();

    public event Action OnWaveGenerated;

    private void GenerateCurrentWave(WaveSO wave)
    {
        foreach (WaveObject waveObject in wave.WaveObjects)
        {
            SpawnLocation spawnLocation = waveObject.SpawnLoc;
            SpawnPoint spawnPoint = GetSpawnPoint(spawnLocation);

            List<GameObject> enemies = ObjectPooler.Instance.GetPool(waveObject.Prefab, waveObject.AmountOfUnits);

            foreach (GameObject enemy in enemies)
            {
                if (spawnLocation == SpawnLocation.Random)
                {
                    spawnPoint = GetSpawnPoint(spawnLocation);
                }

                spawnPoint.UnitsToSpawn.Enqueue(enemy);
            }
        }

        OnWaveGenerated?.Invoke();
    }

    private SpawnPoint GetSpawnPoint(SpawnLocation spawnPoint)
    {
        int index = (int)spawnPoint - 1; // spawnPoints don't contain value for Random, so we remove it

        if (spawnPoint == SpawnLocation.Random)
        {
            index = Random.Range(0, spawnPoints.Count);
        }

        return spawnPoints[index];
    }

    private void Start()
    {
        GenerateCurrentWave(waves[0]);
    }
}