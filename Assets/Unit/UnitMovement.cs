using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitMovement : MonoBehaviour
{
    public Vector3 Destination { get; private set; }

    [SerializeField] [Range(0f, 1000f)] protected float speed = 250f;

    protected Unit Unit;

    protected bool MovingToDestination;
    protected bool DestinationReached;

    protected Coroutine UnitStuckCoroutine;

    [SerializeField] protected float pathNodeDistanceThreshold = 0.5f;
    protected Queue<Vector3> PathToDestination = new();

    private Vector3 _pathNodePos;

    #region UnityMethods

    private void Awake()
    {
        GetUnit();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        Move();
        Rotate();
    }

    #endregion

    public virtual void InitMovement()
    {
        GetUnit();

        Destination = GetDestination();
        PathToDestination = GetPath();

        if (PathToDestination.Count != 0)
        {
            _pathNodePos = PathToDestination.Dequeue();

            Unit.Animator.SetBool(Constants.AnimRunningParam, true);

            DestinationReached = false;
            MovingToDestination = true;
        }
    }

    public virtual void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);

        if (!DestinationReached)
        {
            UnitStuckCoroutine = StartCoroutine(CheckUnitStuck());
        }

        MovingToDestination = false;
    }

    public virtual void StartMovement()
    {
        if (!DestinationReached && PathToDestination.Count != 0)
        {
            if (UnitStuckCoroutine != null)
            {
                StopCoroutine(UnitStuckCoroutine);
            }

            Unit.Animator.SetBool(Constants.AnimRunningParam, true);
            MovingToDestination = true;
        }
    }

    protected virtual void Move()
    {
        if (!MovingToDestination || DestinationReached)
        {
            return;
        }

        float distanceToNode = Vector3.Distance(Unit.transform.position, _pathNodePos);
        if (distanceToNode < pathNodeDistanceThreshold)
        {
            // Reached last node
            if (PathToDestination.Count == 0)
            {
                ReachedTarget();
                return;
            }

            _pathNodePos = PathToDestination.Dequeue();
        }

        Vector3 direction = _pathNodePos - Unit.transform.position;
        direction.Normalize();

        Vector3 velocity = direction * (speed * Time.deltaTime);
        Unit.Rigidbody.velocity = velocity;
    }

    protected virtual void Rotate()
    {
        if (!IsRotationAllowed())
        {
            return;
        }

        Quaternion directionQ = Unit.transform.rotation;

        if (Unit.Rigidbody.velocity != Vector3.zero)
        {
            directionQ = Quaternion.LookRotation(Unit.Rigidbody.velocity);
        }

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected virtual bool IsRotationAllowed()
    {
        return MovingToDestination;
    }

    protected virtual Vector3 GetDestination()
    {
        throw new NotImplementedException();
    }

    protected virtual Queue<Vector3> GetPath()
    {
        return Pathfinding.Instance.GetPath(Unit.transform.position, Destination);
    }

    protected IEnumerator CheckUnitStuck()
    {
        Vector3 currUnitPos = Unit.transform.position;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        if (Vector3.Distance(currUnitPos, Unit.transform.position) < 0.1f)
        {
            InitMovement();
        }
    }

    private void ReachedTarget()
    {
        DestinationReached = true;
        StopMovement();
        InitMovement();
    }

    private void GetUnit() // TODO: optimize
    {
        if (Unit == null)
        {
            Unit = GetComponentInParent<Unit>();
        }
    }
}