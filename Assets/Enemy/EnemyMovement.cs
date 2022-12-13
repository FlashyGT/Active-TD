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

    public override void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);

        if (!DestinationReached && !UnitInCombat())
        {
            UnitStuckCoroutine = StartCoroutine(CheckUnitStuck());
        }

        MovingToDestination = false;
    }

    public override void StartMovement()
    {
        if (!DestinationReached && PathToDestination.Count != 0 && !UnitInCombat())
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
        if (RotationNotAllowed())
        {
            return;
        }

        Quaternion directionQ = Unit.transform.rotation;

        if (UnitInCombat())
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

    protected override bool RotationNotAllowed()
    {
        return !MovingToDestination && Unit.Combat.Targets.Count == 0;
    }

    protected override Vector3 GetCurrentDestination()
    {
        IDamageable target = ActionManager.Instance.GetEnemyActionDestination();

        if (target == null)
        {
            return Unit.transform.position;
        }

        target.OnDeath += TargetDied;
        return target.GetAttackPoint();
    }

    private void TargetDied(IDamageable target)
    {
        InitMovement();
    }

    private bool UnitInCombat()
    {
        return Unit.Combat.Targets.Count > 0;
    }
}