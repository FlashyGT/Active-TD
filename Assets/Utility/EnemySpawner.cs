using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public event Action OnWaveGenerated;

    public int AmountOfUnitsInWave
    {
        get { return _amountOfUnitsInWave; }
        set
        {
            _amountOfUnitsInWave = value;
            if (_amountOfUnitsInWave == 0)
            {
                StartCoroutine(WaveDefeated());
            }
        }
    }

    [SerializeField] private List<SpawnPoint> spawnPoints = new();
    [SerializeField] private List<WaveSO> waves = new();
    [SerializeField] [Range(5f, 60f)] private float timeBetweenWaves = 30f; // night time duration

    private int _currentWave = 0;
    private int _amountOfUnitsInWave;

    #region UnityMethods

    private void Start()
    {
        GenerateCurrentWave(_currentWave);
    }

    #endregion

    private void GenerateCurrentWave(int currentWave)
    {
        WaveSO wave = waves[currentWave];

        foreach (WaveObject waveObject in wave.WaveObjects)
        {
            List<GameObject> enemies = ObjectPooler.Instance.GetPool(waveObject.Prefab, waveObject.AmountOfUnits);

            AddEnemiesToSpawnPoints(waveObject.SpawnLoc, enemies);
        }

        OnWaveGenerated?.Invoke();
    }

    private void AddEnemiesToSpawnPoints(SpawnLocation defaultSpawnLoc, List<GameObject> enemies)
    {
        SpawnPoint spawnPoint = GetSpawnPoint(defaultSpawnLoc);
        foreach (GameObject enemy in enemies)
        {
            if (defaultSpawnLoc == SpawnLocation.Random)
            {
                spawnPoint = GetSpawnPoint(defaultSpawnLoc);
            }

            AmountOfUnitsInWave++;
            spawnPoint.UnitsToSpawn.Enqueue(enemy);
        }
    }

    private SpawnPoint GetSpawnPoint(SpawnLocation spawnPoint)
    {
        int index = (int)spawnPoint - 1; // spawnPoints don't contain value for Random, so we remove it

        if (spawnPoint == SpawnLocation.Random)
        {
            index = Random.Range(0, spawnPoints.Count - 1);
        }

        return spawnPoints[index];
    }

    private IEnumerator WaveDefeated()
    {
        _currentWave++;
        yield return new WaitForSeconds(timeBetweenWaves);
        if (waves.Count != _currentWave)
        {
            GenerateCurrentWave(_currentWave);
        }
    }
}