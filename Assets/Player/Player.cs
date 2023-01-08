using System;
using UnityEngine;

public class Player : Unit
{
    private Vector3 _startPos;
    
    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;
    }

    public override void OnDead()
    {
        base.OnDead();
        GameManager.Instance.GameLost();
    }

    protected override void Reset()
    {
        transform.position = _startPos;
        base.Reset();
        Animator.Rebind();
        Animator.Play("Idle");
    }
}