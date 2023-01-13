using System;
using System.Collections;
using UnityEngine;

public class Player : Unit
{
    private Vector3 _startPos;

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;

        StartCoroutine(StartHealthRegeneration());
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

    private IEnumerator StartHealthRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            
            if (!Combat.IsUnitInCombat() && !ObjectHealth.FullHealth())
            {
                GameManager.Instance.HealObject(this, 10);   
            }
        }
    }
}