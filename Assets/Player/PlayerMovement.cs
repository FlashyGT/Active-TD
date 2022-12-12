using UnityEngine;

public class PlayerMovement : UnitMovement
{
    [SerializeField] private bool testing; // TODO: remove

    private Touch _touch;
    private bool _touching;
    private Vector2 _touchDirection;
    private Vector2 _touchStartPos;
    private Vector3 _touchVelocity;

    #region UnityMethods

    protected override void Start()
    {
    }

    protected override void Update()
    {
        if (!testing)
        {
            _touching = Input.touchCount > 0;
            GetTouch();
        }
        else
        {
            DesktopControls();
        }
    }

    #endregion

    public override void StopMovement()
    {
        Unit.Rigidbody.velocity = Vector3.zero;
        Unit.Rigidbody.angularVelocity = Vector3.zero;
        Unit.Animator.SetBool(Constants.AnimRunningParam, false);
    }

    protected override void Move()
    {
        if (!_touching)
        {
            StopMovement();
            return;
        }

        Vector3 newVelocity = _touchVelocity * (speed * Time.deltaTime);
        Unit.Rigidbody.velocity = newVelocity;
        Unit.Animator.SetBool(Constants.AnimRunningParam, true);
    }

    protected override void Rotate()
    {
        if (RotationNotAllowed())
        {
            return;
        }

        Quaternion directionQ = Unit.transform.rotation;

        if (Unit.Combat.Targets.Count != 0)
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
        return !_touching && Unit.Combat.Targets.Count == 0;
    }

    private void GetTouch()
    {
        if (_touching)
        {
            _touch = Input.GetTouch(0);

            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    _touchStartPos = _touch.position;
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    _touchDirection = _touch.position - _touchStartPos;
                    _touchVelocity = new Vector3(_touchDirection.x, 0f, _touchDirection.y);
                    _touchVelocity.Normalize();
                    break;
            }
        }
    }

    private void DesktopControls()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        if (xMove == 0f && zMove == 0f)
        {
            _touching = false;
            return;
        }

        _touchVelocity = new Vector3(xMove, 0f, zMove);
        _touchVelocity.Normalize();
        _touching = true;
    }
}