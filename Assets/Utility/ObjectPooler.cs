using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    private Transform _poolParent;

    private Dictionary<string, Transform> _poolObjectParents = new();
    private Dictionary<GameObject, List<GameObject>> _pools = new();

    private int _defaultPoolSize = 5;

    #region UnityMethods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _poolParent = new GameObject("Object Pools").transform;
    }

    #endregion

    public GameObject GetObject(GameObject poolObject)
    {
        if (!_pools.ContainsKey(poolObject))
        {
            CreatePool(poolObject, _defaultPoolSize);
        }

        return GetPoolObject(poolObject);
    }

    private void CreatePool(GameObject poolObject, int amountOfObjects)
    {
        CreatePoolObjectParent(poolObject);

        List<GameObject> pool = new();
        AddObjectsToPool(poolObject, pool, amountOfObjects);

        _pools.Add(poolObject, pool);
    }

    private void CreatePoolObjectParent(GameObject poolObject)
    {
        string parentName = poolObject.name;
        GameObject parent = new GameObject(parentName);
        parent.transform.SetParent(_poolParent);
        _poolObjectParents.Add(parentName, parent.transform);
    }

    private void AddObjectsToPool(GameObject poolObject, List<GameObject> poolToAddTo, int amountOfObjects)
    {
        List<GameObject> currPool = poolToAddTo;

        Transform objectParent = _poolObjectParents[poolObject.name];

        for (int x = 0; x < amountOfObjects; x++)
        {
            GameObject currObject = Instantiate(poolObject, objectParent);
            currObject.SetActive(false);
            currPool.Add(currObject);
        }
    }

    private GameObject GetPoolObject(GameObject poolObject)
    {
        List<GameObject> objectPool = _pools[poolObject];

        int poolIndex;
        for (poolIndex = 0; poolIndex < objectPool.Count; poolIndex++)
        {
            GameObject currObj = objectPool[poolIndex];
            if (!currObj.activeInHierarchy)
            {
                return currObj;
            }
        }

        AddObjectsToPool(poolObject, objectPool, _defaultPoolSize);

        // Get next object in pool that was just added
        return objectPool[poolIndex + 1];
    }
}