using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangedCombat : UnitCombat
{
    [SerializeField] private Transform projectileOrigin;

    private CombatUnit _combatUnit;

    private Projectile _projectile;
    private Vector3 _projectileOriginPos;
    private int _projectileDamage;

    private int _maxAttacksBeforeResupply;
    private int _currentAttackAmount;
    
    private bool _waitingForFood;

    private Coroutine _foodCoroutine;
    
    #region UnityMethods

    protected override void Awake()
    {
        base.Awake();
        _waitingForFood = false;
    }

    protected override void Start()
    {
        _combatUnit = GetComponentInParent<CombatUnit>();
        Unit = _combatUnit;
        Unit.OnObjRespawn.AddListener(Targets.Clear);
        InitProjectile();
        GameManager.Instance.GameStarted += Reset;
        HasFinishedLoading = true;
    }

    #endregion

    public override bool IsUnitInCombat()
    {
        return base.IsUnitInCombat() && _currentAttackAmount != 0 && !_waitingForFood;
    }

    public override void Attack()
    {
        if (CurrentTarget == null)
        {
            return;
        }
        
        _projectileOriginPos = projectileOrigin.position;
        _projectile.Launch(CurrentTarget, _projectileOriginPos, _projectileDamage);

        HandleAmmo();
    }

    protected override bool AllowedToStartCombat()
    {
        return base.AllowedToStartCombat() && _currentAttackAmount != 0 && !_waitingForFood;
    }

    private void HandleAmmo()
    {
        _currentAttackAmount--;
        if (_currentAttackAmount == 0)
        {
            StopCombat();
            _combatUnit.StartAction(UnitActionType.Delivery, UnitActionItem.Ammo, AmmoActionCompleted);
        }
    }
    
    private void AmmoActionCompleted()
    {
        ResetAttacks();
        StartCombat();
    }

    private void InitProjectile()
    {
        _projectileDamage = unitCombatSo.Damage;
        _projectileOriginPos = projectileOrigin.position;
        _maxAttacksBeforeResupply = unitCombatSo.AttacksBeforeResupply;
        ResetAttacks();

        GameObject projectileGO = Instantiate(unitCombatSo.Projectile, _projectileOriginPos, Quaternion.identity, gameObject.transform);
        _projectile = projectileGO.GetComponent<Projectile>();
    }

    private void ResetAttacks()
    {
        _currentAttackAmount = _maxAttacksBeforeResupply;
    }

    private IEnumerator FoodRequired()
    {
        yield return new WaitForSeconds(unitCombatSo.FoodRequiredAfter);
        _waitingForFood = true;
        StopCombat();
        _combatUnit.StartAction(UnitActionType.Delivery, UnitActionItem.Food, FoodActionCompleted);
    }
    
    private void FoodActionCompleted()
    {
        _waitingForFood = false;
        StartCombat();
        _foodCoroutine = StartCoroutine(FoodRequired());
    }

    private void Reset()
    {
        if (_foodCoroutine != null)
        {
            StopCoroutine(_foodCoroutine);
        }

        _waitingForFood = false;
        _foodCoroutine = StartCoroutine(FoodRequired());
        ResetAttacks();
    }
}