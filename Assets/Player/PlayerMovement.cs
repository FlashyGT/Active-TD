using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0f, 1000f)] private float speed = 250f;

    private Unit _unit;

    private Touch _touch;
    private bool _touching;
    private Vector2 _touchDirection;
    private Vector2 _touchStartPos;
    private Vector3 _touchVelocity;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        _touching = Input.touchCount > 0;
        GetTouch();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
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

    private void Move()
    {
        Vector3 newVelocity = Vector3.zero;
        _unit.Animator.SetBool(Constants.AnimRunningParam, false);

        if (_touching)
        {
            _unit.Animator.SetBool(Constants.AnimRunningParam, true);
            newVelocity = _touchVelocity * (speed * Time.deltaTime);
        }

        _unit.Rigidbody.velocity = newVelocity;
    }

    private void Rotate()
    {
        Quaternion directionQ = transform.rotation;

        if (_touching)
        {
            directionQ = Quaternion.LookRotation(_unit.Rigidbody.velocity);
        }

        _unit.Rigidbody.angularVelocity = Vector3.zero;
        _unit.Rigidbody.MoveRotation(directionQ);
    }
}