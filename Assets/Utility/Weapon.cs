using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private const string attackParameter = "Attack";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        animator.SetBool(attackParameter, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        animator.SetBool(attackParameter, false);
    }
}
