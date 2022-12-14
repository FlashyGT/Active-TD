using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    private float _secondsToCompleteAction = 1f;
    private Coroutine _actionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        _actionCoroutine = StartCoroutine(Action());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(_actionCoroutine);
    }

    private IEnumerator Action()
    {
        Debug.Log("Action started");
        yield return new WaitForSeconds(_secondsToCompleteAction);
        Debug.Log("Action ended");
    }
}