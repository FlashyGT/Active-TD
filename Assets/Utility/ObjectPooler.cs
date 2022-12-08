using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    private Transform _poolParent;
    private Dictionary<string, Transform> _poolObjectParents = new();

    private Dictionary<GameObject, Queue<GameObject>> _pools = new();

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

    public List<GameObject> GetPool(GameObject poolObject, int amountOfObjects)
    {
        if (!_pools.ContainsKey(poolObject))
        {
            CreatePool(poolObject, amountOfObjects);
        }

        return GetObjects(poolObject, amountOfObjects);
    }

    private void CreatePool(GameObject poolObject, int amountOfObjects)
    {
        CreatePoolObjectParent(poolObject);

        Queue<GameObject> pool = new();
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

    private void AddObjectsToPool(GameObject poolObject, Queue<GameObject> poolToAddTo, int amountOfObjects)
    {
        Queue<GameObject> currPool = poolToAddTo;

        if (currPool == null)
        {
            currPool = _pools[poolObject];
        }

        Transform objectParent = _poolObjectParents[poolObject.name];

        for (int x = 0; x < amountOfObjects; x++)
        {
            GameObject currObject = Instantiate(poolObject, objectParent);
            currObject.SetActive(false);
            currPool.Enqueue(currObject);
        }
    }

    private List<GameObject> GetObjects(GameObject poolObject, int amountOfObjects)
    {
        List<GameObject> objects = new();
        Queue<GameObject> objectPool = _pools[poolObject];
        int amountOfObjectsInPool = objectPool.Count;

        if (amountOfObjects > amountOfObjectsInPool)
        {
            AddObjectsToPool(poolObject, null, amountOfObjects - amountOfObjectsInPool);
        }

        for (int x = 0; x < amountOfObjects; x++)
        {
            objects.Add(objectPool.Dequeue());
        }

        return objects;
    }
}