using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] [Range(0f, 1000f)] protected float speed = 250f;

    protected Unit Unit;

    private Vector3 _destination;

    private List<Vector3> _pathToDestination = new();
    private Vector3 _pathNodePos;
    private int _pathNodeIndex;

    private bool _movingToDestination;
    private bool _destinationReached;

    #region UnityMethods

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0)) // TODO: Remove this
        {
            _destination = GameManager.Instance.GetWorldPositionOnPlane(Input.mousePosition, 0f);
            _pathToDestination = GameManager.Instance.GetPath(Unit.transform.position, _destination);

            InitMovement();
        }
    }

    protected virtual void FixedUpdate()
    {
        Move();
        Rotate();
    }

    #endregion

    public virtual void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);
        _movingToDestination = false;
    }

    public void StartMovement()
    {
        if (!_destinationReached && _pathToDestination.Count != 0)
        {
            Unit.Animator.SetBool(Constants.AnimRunningParam, true);
            _movingToDestination = true;
        }
    }

    protected virtual void Move()
    {
        if (!_movingToDestination || _destinationReached)
        {
            return;
        }

        float distanceToNode = Vector3.Distance(Unit.transform.position, _pathNodePos);
        if (distanceToNode < 0.25f)
        {
            // Reached last node
            if (_pathNodeIndex == _pathToDestination.Count - 1)
            {
                ReachedTarget();
                return;
            }

            _pathNodeIndex++;
            _pathNodePos = _pathToDestination[_pathNodeIndex];
        }

        Vector3 direction = _pathNodePos - Unit.transform.position;
        direction.Normalize();

        Vector3 velocity = direction * (speed * Time.deltaTime);
        Unit.Rigidbody.velocity = velocity;
    }

    protected virtual void Rotate()
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
            directionQ = Quaternion.LookRotation(Unit.Combat.Targets[0].transform.position - Unit.transform.position);
        }

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected virtual bool RotationNotAllowed()
    {
        return !_movingToDestination && Unit.Combat.Targets.Count == 0;
    }

    private void InitMovement()
    {
        if (_pathToDestination.Count != 0)
        {
            _pathNodeIndex = 0;
            _pathNodePos = _pathToDestination[_pathNodeIndex];
            Unit.Animator.SetBool(Constants.AnimRunningParam, true);
            _destinationReached = false;
            _movingToDestination = true;
        }
    }

    private void ReachedTarget()
    {
        _destinationReached = true;
        StopMovement();
    }
}