using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Farm : MonoBehaviour, IUpgradeable
{
    [SerializeField] private List<Garden> gardens;

    #region UnityMethods

    private void Start()
    {
        KeepTrackOfGardens();
    }

    #endregion

    public void Upgrade()
    {
        throw new NotImplementedException();
    }

    public Garden GetGarden()
    {
        if (gardens.Count == 0)
        {
            return null;
        }

        return gardens[0];
    }

    private void KeepTrackOfGardens()
    {
        foreach (Garden garden in gardens)
        {
            garden.OnObjDeath.AddListener(GardenDied);
        }
    }

    private void GardenDied()
    {
        gardens.RemoveAt(0);

        if (gardens.Count == 0)
        {
            GameManager.Instance.GameLost();
        }
    }
}