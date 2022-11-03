using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 1000f)]
    private float speed = 10f;

    private bool touching;
    private Touch touch;
    private Vector2 touchStartPos;
    private Vector2 touchDirection;
    private Vector3 touchVelocity;

    private const string runningParameter = "Running";

    [SerializeField]
    private Animator animator;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        touching = Input.touchCount > 0;
        GetTouch();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void GetTouch()
    {
        if (touching)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    touchDirection = touch.position - touchStartPos;
                    touchVelocity = new Vector3(touchDirection.x, 0f, touchDirection.y);
                    touchVelocity.Normalize();
                    break;
            }
        }
    }

    private void Move()
    {
        Vector3 newVelocity = Vector3.zero;
        animator.SetBool(runningParameter, false);

        if (touching)
        {
            animator.SetBool(runningParameter, true);
            newVelocity = touchVelocity * speed * Time.deltaTime;
        }

        rigidBody.velocity = newVelocity;
    }

    private void Rotate()
    {
        Quaternion directionQ = transform.rotation;
        if (touching)
        {
            directionQ = Quaternion.LookRotation(rigidBody.velocity);
        }
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.MoveRotation(directionQ);
    }
}
