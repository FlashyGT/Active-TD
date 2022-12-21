using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Queue<GameObject> UnitsToSpawn { get; private set; }

    [SerializeField] private int secondsBeforeNextUnit = 1;

    private EnemySpawner _enemySpawner;
    private ObjectPooler _objectPooler;

    private void Awake()
    {
        UnitsToSpawn = new Queue<GameObject>();

        _enemySpawner = GetComponentInParent<EnemySpawner>();
        _enemySpawner.OnWaveGenerated += StartSpawning;
    }

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnUnit());
    }

    private IEnumerator SpawnUnit()
    {
        if (UnitsToSpawn.Count == 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(secondsBeforeNextUnit);

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