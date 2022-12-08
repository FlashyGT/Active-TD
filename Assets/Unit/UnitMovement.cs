using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] [Range(0f, 1000f)] protected float speed = 250f;

    protected Unit Unit;

    protected bool MovingToDestination;

    private List<Vector3> _pathToDestination = new();
    private Vector3 _pathNodePos;
    private int _pathNodeIndex;

    private bool _destinationReached;

    private Coroutine _unitStuckCoroutine;

    #region UnityMethods

    private void Awake()
    {
        GetUnit();
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

    public void InitMovement()
    {
        GetUnit(); // TODO: optimize

        _pathToDestination = Pathfinding.Instance.GetPath(Unit.transform.position, GetCurrentDestination());
        if (_pathToDestination.Count != 0)
        {
            _pathNodeIndex = 0;
            _pathNodePos = _pathToDestination[_pathNodeIndex];

            Unit.Animator.SetBool(Constants.AnimRunningParam, true);

            _destinationReached = false;
            MovingToDestination = true;
        }
    }

    public virtual void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);

        if (!_destinationReached)
        {
            _unitStuckCoroutine = StartCoroutine(CheckUnitStuck());
        }

        MovingToDestination = false;
    }

    public void StartMovement()
    {
        if (!_destinationReached && _pathToDestination.Count != 0)
        {
            if (_unitStuckCoroutine != null)
            {
                StopCoroutine(_unitStuckCoroutine);
            }

            Unit.Animator.SetBool(Constants.AnimRunningParam, true);
            MovingToDestination = true;
        }
    }

    protected virtual void Move()
    {
        if (!MovingToDestination || _destinationReached)
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

        Unit.Rigidbody.MoveRotation(directionQ);
    }

    protected virtual bool RotationNotAllowed()
    {
        return !MovingToDestination;
    }

    private Vector3 GetCurrentDestination()
    {
        return new Vector3(0f, 0f, 41.16f); // TODO: implement
    }

    private void ReachedTarget()
    {
        _destinationReached = true;
        StopMovement();
    }

    private void GetUnit()
    {
        if (Unit == null)
        {
            Unit = GetComponentInParent<Unit>();
        }
    }

    private IEnumerator CheckUnitStuck()
    {
        Vector3 currUnitPos = Unit.transform.position;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        if (Vector3.Distance(currUnitPos, Unit.transform.position) < 0.1f)
        {
            InitMovement();
        }
    }
}