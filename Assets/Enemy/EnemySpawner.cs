using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public event Action OnWaveGenerated;
    public event Action OnWaveDefeated;

    public bool WaveInProgress => AmountOfUnitsInWave > 0;
    
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
    [SerializeField] [Range(10f, 60f)] private float timeBetweenWaves = 30f; // night time duration

    private int _currentWave = 0;
    private int _amountOfUnitsInWave;

    #region UnityMethods

    private void Start()
    {
        GameManager.Instance.GameStarted += StartGenerating;
    }

    #endregion

    private void StartGenerating()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            spawnPoint.ResetSpawning();
        }
        GenerateCurrentWave(_currentWave);
    }

    private void GenerateCurrentWave(int currentWave)
    {
        WaveSO wave = waves[currentWave];

        foreach (WaveObject waveObject in wave.WaveObjects)
        {
            AddEnemiesToSpawnPoints(waveObject.SpawnLoc, waveObject.Prefab, waveObject.AmountOfUnits);
        }

        OnWaveGenerated?.Invoke();
    }

    private void AddEnemiesToSpawnPoints(SpawnLocation defaultSpawnLoc, GameObject enemy, int amount)
    {
        SpawnPoint spawnPoint = GetSpawnPoint(defaultSpawnLoc);

        for (int x = 0; x < amount; x++)
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
        timeBetweenWaves += 5;
        OnWaveDefeated?.Invoke();
        yield return new WaitForSeconds(timeBetweenWaves);
        if (waves.Count != _currentWave)
        {
            GenerateCurrentWave(_currentWave);
        }
    }
}