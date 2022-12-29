using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangedCombat : UnitCombat
{
    [SerializeField] private Transform projectileOrigin;
    
    private Projectile _projectile;
    private Vector3 _projectileOriginPos;

    #region UnityMethods
    
    protected override void Start()
    {
        base.Start();
        InitProjectile();
    }
    
    #endregion

    public override void Attack()
    {
        _projectileOriginPos = projectileOrigin.position;
        _projectile.Launch(CurrentTarget, _projectileOriginPos, weaponSo.damage);
    }

    private void InitProjectile()
    {
        _projectileOriginPos = projectileOrigin.position;
        GameObject projectileGO = Instantiate(weaponSo.projectile, _projectileOriginPos, Quaternion.identity, gameObject.transform);
        _projectile = projectileGO.GetComponent<Projectile>();
    }
}