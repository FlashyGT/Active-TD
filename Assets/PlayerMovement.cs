using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 1000f)]
    private float speed = 10f;

    private Touch touch;
    private Vector2 touchStartPos;
    private Vector2 touchDirection;
    private Vector3 touchVelocity;

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
        GetTouch();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void GetTouch()
    {
        if (Input.touchCount > 0)
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
        if (Input.touchCount > 0)
        {
            newVelocity = touchVelocity * speed * Time.deltaTime;
        }
        rigidBody.velocity = newVelocity;
    }

    private void Rotate()
    {
        Quaternion directionQ = Quaternion.LookRotation(rigidBody.velocity);
        rigidBody.MoveRotation(directionQ);
    }
}
