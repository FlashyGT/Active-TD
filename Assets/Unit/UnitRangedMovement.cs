using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangedMovement : UnitMovement
{
    #region UnityMethods

    protected override void FixedUpdate()
    {
        Rotate();
    }

    #endregion

    protected override void Rotate()
    {
        if (!IsRotationAllowed())
        {
            return;
        }

        Vector3 targetPos = Unit.Combat.CurrentTarget.GetGameObject().transform.position;
        Quaternion directionQ = Quaternion.LookRotation(targetPos - Unit.transform.position);

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected override bool IsRotationAllowed()
    {
        return IsUnitInCombat();
    }

    private bool IsUnitInCombat()
    {
        return Unit.Combat.TargetCount > 0;
    }
}