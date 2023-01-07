using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangedMovement : UnitMovement
{
    #region UnityMethods

    protected override void FixedUpdate()
    {
        if (Unit.HasFinishedLoading)
        {
            Rotate();   
        }
    }

    #endregion
    
    public override void InitMovement()
    {
        
    }

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
        return Unit.Combat.IsUnitInCombat();
    }
}