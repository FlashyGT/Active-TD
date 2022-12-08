using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : UnitMovement
{
    protected override void Rotate()
    {
        if (RotationNotAllowed())
        {
            return;
        }

        Quaternion directionQ = Unit.transform.rotation;

        if (Unit.Rigidbody.velocity != Vector3.zero)
        {
            directionQ = Quaternion.LookRotation(Unit.Rigidbody.velocity);
        }
        else if (Unit.Combat.Targets.Count != 0)
        {
            Vector3 targetPos = Unit.Combat.Targets[0].GetGameObject().transform.position;
            directionQ = Quaternion.LookRotation(targetPos - Unit.transform.position);
        }

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected override bool RotationNotAllowed()
    {
        return !MovingToDestination && Unit.Combat.Targets.Count == 0;
    }
}