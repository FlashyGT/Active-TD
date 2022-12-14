using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    #region UnityMethods

    private void Start()
    {
        transform.rotation = GameManager.Instance.MainCamera.transform.rotation;
    }

    #endregion
}