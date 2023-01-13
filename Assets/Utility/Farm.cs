using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Farm : MonoBehaviour
{
    public Vector3 WellLocation { get; private set; }
    
    [SerializeField] private Well well;
    
    private Queue<DamageableBuilding> _gardens = new();

    #region UnityMethods

    private void Start()
    {
        WellLocation = well.GetUAMLocation();
    }

    #endregion

    public void AddGarden(Garden garden)
    {
        if (_gardens.Contains(garden))
        {
            return;
        }
        
        _gardens.Enqueue(garden);
        garden.OnDeath += GardenDied;
    }

    public DamageableBuilding GetGarden()
    {
        if (_gardens.Count == 0)
        {
            return null;
        }

        return _gardens.Peek();
    }

    private void GardenDied(IDamageable garden)
    {
        _gardens = GameManager.Instance.RemoveItemFromQueue((DamageableBuilding)garden, _gardens);

        if (_gardens.Count == 0)
        {
            GameManager.Instance.GameLost();
        }
    }
}