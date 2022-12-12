using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public override void OnDead()
    {
        base.OnDead();
        GameManager.Instance.GameLost();
    }
}