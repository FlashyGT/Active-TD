using System;
using UnityEngine;

public class UnitCollisionBlocker : MonoBehaviour
{
    // Set to true after Start() has finished
    public bool HasFinishedLoading { get; protected set; }

    [SerializeField] private CapsuleCollider unitCollider;
    private CapsuleCollider _unitCollisionBlocker;

    private Unit _unit;

    private int _layerMask;

    #region UnityMethods

    private void Awake()
    {
        IgnoreSelfCollision();
    }

    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
        _layerMask = LayerMask.GetMask(Constants.LayerUnitCollisionBlocker);
        HasFinishedLoading = true;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 unitPosition = _unit.transform.position;
        Vector3 raycastOrigin = new Vector3(unitPosition.x, 0.5f, unitPosition.z);

        if (Physics.Raycast(raycastOrigin, _unit.transform.forward, 1f, _layerMask))
        {
            _unit.Movement.StopMovement();
        }
    }

    private void IgnoreSelfCollision()
    {
        _unitCollisionBlocker = GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(unitCollider, _unitCollisionBlocker, true);
    }
}