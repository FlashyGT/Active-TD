using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Farm : MonoBehaviour, IUpgradeable
{
    public Vector3 WellLocation { get; private set; }

    private Queue<IDamageable> _gardens = new();

    #region UnityMethods

    private void Awake()
    {
        WellLocation = ActionManager.Instance.Well.GetUAMLocation();
    }

    #endregion

    public void AddGarden(Garden garden)
    {
        _gardens.Enqueue(garden);
        garden.OnDeath += GardenDied;
    }

    public IDamageable GetGarden()
    {
        if (_gardens.Count == 0)
        {
            return null;
        }

        return _gardens.Peek();
    }

    public void Upgrade()
    {
        throw new NotImplementedException();
    }

    private void GardenDied(IDamageable garden)
    {
        _gardens = GameManager.Instance.RemoveItemFromQueue(garden, _gardens);

        if (_gardens.Count == 0)
        {
            GameManager.Instance.GameLost();
        }
    }
}