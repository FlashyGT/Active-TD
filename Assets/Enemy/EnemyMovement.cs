using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : UnitMovement
{
    protected override void Start()
    {
        Unit.Combat.OnCombatStarted += StopMovement;
        Unit.Combat.OnCombatEnded += StartMovement;
    }

    public override void InitMovement()
    {
        if (!IsUnitInCombat())
        {
            base.InitMovement();
        }
    }

    public override void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);

        if (!DestinationReached && !IsUnitInCombat())
        {
            UnitStuckCoroutine = StartCoroutine(CheckUnitStuck());
        }

        MovingToDestination = false;
    }

    public override void StartMovement()
    {
        if (!DestinationReached && PathToDestination.Count != 0 && !IsUnitInCombat())
        {
            if (UnitStuckCoroutine != null)
            {
                StopCoroutine(UnitStuckCoroutine);
            }

            Unit.Animator.SetBool(Constants.AnimRunningParam, true);
            MovingToDestination = true;
        }
    }

    protected override void Rotate()
    {
        if (IsRotationAllowed())
        {
            return;
        }

        Quaternion directionQ = Unit.transform.rotation;

        if (IsUnitInCombat())
        {
            Vector3 targetPos = Unit.Combat.Targets[0].GetGameObject().transform.position;
            directionQ = Quaternion.LookRotation(targetPos - Unit.transform.position);
        }
        else if (Unit.Rigidbody.velocity != Vector3.zero)
        {
            directionQ = Quaternion.LookRotation(Unit.Rigidbody.velocity);
        }

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected override bool IsRotationAllowed()
    {
        return !MovingToDestination && !IsUnitInCombat();
    }

    protected override Vector3 GetDestination()
    {
        return ActionManager.Instance.GetEnemyActionDestination(Unit);
    }

    private bool IsUnitInCombat()
    {
        return Unit.Combat.Targets.Count > 0;
    }
}