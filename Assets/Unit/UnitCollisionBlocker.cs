using UnityEngine;

public class UnitCollisionBlocker : MonoBehaviour
{
    [SerializeField] private CapsuleCollider unitCollider;
    private CapsuleCollider _unitCollisionBlocker;

    private Unit _unit;

    private int _layerMask;

    #region UnityMethods

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
        _unitCollisionBlocker = GetComponent<CapsuleCollider>();

        Physics.IgnoreCollision(unitCollider, _unitCollisionBlocker, true);
    }

    private void Start()
    {
        _layerMask = LayerMask.GetMask(Constants.LayerUnitCollisionBlocker);
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

    private void OnCollisionExit(Collision other)
    {
        _unit.Movement.StartMovement();
    }
}