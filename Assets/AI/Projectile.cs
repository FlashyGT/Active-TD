using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("An angle the projectile will shoot from")] 
    [Range(0f, 80f)] [SerializeField] private float angle;

    [Tooltip("Height which the projectile will reach to hit the target")]
    [Range(0f, 5f)] [SerializeField] private float height;

    private IDamageable _target;
    private int _projectileDamage;
    
    private float _initialVelocity;
    private float _timeToTarget;
    private float _angleToTarget;
    private float _height;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private Vector3 _directionToTarget;
    private Vector3 _groundDirectionToTarget;

    private float _gravity;

    #region UnityMethods

    private void Awake()
    {
        _gravity = -Physics.gravity.y;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    #endregion

    public void Launch(IDamageable target, Vector3 projectileStartPos, int projectileDamage)
    {
        InitLaunchValues(target, projectileStartPos, projectileDamage);
        CalculatePath();
        StartCoroutine(Movement(_groundDirectionToTarget.normalized));
    }

    private void InitLaunchValues(IDamageable target, Vector3 projectileStartPos, int projectileDamage)
    {
        gameObject.SetActive(true);
        
        _target = target;
        _projectileDamage = projectileDamage;
        
        _targetPos = target.GetAttackPoint();
        _startPos = projectileStartPos;
        transform.position = projectileStartPos;
        
        _directionToTarget = _targetPos - _startPos;
        _groundDirectionToTarget = new Vector3(_directionToTarget.x, 0f, _directionToTarget.z);
    }

    private void CalculatePath()
    {
        Vector3 target = new Vector3(_groundDirectionToTarget.magnitude, _directionToTarget.y, 0f);

        if (height == 0)
        {
            _angleToTarget = angle * Mathf.Deg2Rad;
            CalculatePathWithAngle(target);
        }
        else
        {
            _height = _targetPos.y + height;
            CalculatePathWithHeight(target);
        }
    }

    private void CalculatePathWithAngle(Vector3 direction)
    {
        float xt = direction.x;
        float yt = direction.y;

        float v1 = Mathf.Pow(xt, 2) * _gravity;
        float v2 = 2 * xt * Mathf.Sin(_angleToTarget) * Mathf.Cos(_angleToTarget);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(_angleToTarget), 2);

        _initialVelocity = Mathf.Sqrt(v1 / (v2 - v3));

        _timeToTarget = xt / (_initialVelocity * Mathf.Cos(_angleToTarget));
    }

    private void CalculatePathWithHeight(Vector3 direction)
    {
        float xt = direction.x;
        float yt = direction.y;

        float b = Mathf.Sqrt(2 * _gravity * _height);
        float a = -0.5f * _gravity;
        float c = -yt;

        float tPlus = QuadraticEquation(a, b, c, 1);
        float tMin = QuadraticEquation(a, b, c, -1);
        _timeToTarget = tPlus > tMin ? tPlus : tMin;

        _angleToTarget = Mathf.Atan(b * _timeToTarget / xt);
        _initialVelocity = b / Mathf.Sin(_angleToTarget);
    }

    private float QuadraticEquation(float a, float b, float c, int sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private IEnumerator Movement(Vector3 direction)
    {
        float timeInFlight = 0;
        while (timeInFlight < _timeToTarget)
        {
            float x = _initialVelocity * timeInFlight * Mathf.Cos(_angleToTarget);
            float y = _initialVelocity * timeInFlight * Mathf.Sin(_angleToTarget) -
                      0.5f * _gravity * Mathf.Pow(timeInFlight, 2);
            transform.position = _startPos + direction * x + Vector3.up * y;

            timeInFlight += Time.deltaTime;
            yield return null;
        }

        MovementFinished();
    }

    private void MovementFinished()
    {
        gameObject.SetActive(false);
        GameManager.Instance.DamageObject(_target, _projectileDamage);
    }
}