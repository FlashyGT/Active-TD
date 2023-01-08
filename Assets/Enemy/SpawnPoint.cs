using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    public Queue<GameObject> UnitsToSpawn { get; private set; }

    [SerializeField] private int secondsBeforeNextUnit = 1;

    private Coroutine _spawningCoroutine;
    
    private EnemySpawner _enemySpawner;
    private ObjectPooler _objectPooler;

    #region UnityMethods
    private void Awake()
    {
        UnitsToSpawn = new Queue<GameObject>();
    }

    private void Start()
    {
        _enemySpawner = GetComponentInParent<EnemySpawner>();
        _enemySpawner.OnWaveGenerated += StartSpawning;
        _objectPooler = ObjectPooler.Instance;
    }
    
    #endregion

    public void ResetSpawning()
    {
        _spawningCoroutine = null;
        UnitsToSpawn.Clear();
    }
    
    private void StartSpawning()
    {
        _spawningCoroutine = StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        if (UnitsToSpawn.Count == 0)
        {
            yield break;
        }

        float delay = Random.Range(secondsBeforeNextUnit / 2, secondsBeforeNextUnit);
        yield return new WaitForSeconds(delay);

        GameObject unitGo = _objectPooler.GetObject(UnitsToSpawn.Dequeue());
        Unit unit = unitGo.GetComponentInDirectChildren<Unit>(true); // TODO: optimize

        unitGo.SetActive(true);
        unit.transform.position = transform.position;
        unit.OnObjDeath.AddListener(RemoveUnitInWave);
        unit.InitUnit();
    }

    private void RemoveUnitInWave()
    {
        _enemySpawner.AmountOfUnitsInWave--;
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(SpawnUnit());
    }
}