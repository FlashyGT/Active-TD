using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }

    [SerializeField] private Barricade barricade;
    [SerializeField] private Farm farm;

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
    }

    #endregion

    public IDamageable GetEnemyActionDestination()
    {
        if (!barricade.ObjectHealth.IsDead())
        {
            return barricade;
        }

        return farm.GetGarden();
    }
}