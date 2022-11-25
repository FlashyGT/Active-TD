using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = GameManager.Instance.Player.transform.position + offset;
    }
}