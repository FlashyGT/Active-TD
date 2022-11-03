using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField]
    private Transform playerPos;

    private void Start()
    {
        offset = transform.position - playerPos.position;
    }

    private void LateUpdate()
    {
        transform.position = playerPos.position + offset;
    }
}
