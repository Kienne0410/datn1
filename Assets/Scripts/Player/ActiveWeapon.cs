using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private Action<InputAction.CallbackContext> _StartAttacking;
    private Action<InputAction.CallbackContext> _StopAttacking;
    private bool attackButtonDown, isAttacking = false;
    public bool autoAttack = false;
    public LayerMask LayerMask;
    [SerializeField] private MoveFollow _moveFollow;

    protected override void Awake() {
        base.Awake();
        playerControls = InputManager.Instance.playerControls;
        _StartAttacking = _ => StartAttacking();
        _StopAttacking = _ => StopAttacking();
        playerControls.Combat.Attack.started += _StartAttacking;
        playerControls.Combat.Attack.canceled += _StopAttacking;
    }

    private void OnDestroy()
    {
        playerControls.Combat.Attack.started -= _StartAttacking;
        playerControls.Combat.Attack.canceled -= _StopAttacking;
    }
    private void Start()
    {
        
        AttackCooldown();
    }

    private void Update() {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;

        AttackCooldown();
        timeBetweenAttacks = 1/((CurrentActiveWeapon as IWeapon).GetWeaponInfo().attackSpeed);
    }

    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown() {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack() {
        if (autoAttack)
        {
            FindNearestEnemy();
        }
        else
        {
            if (attackButtonDown && !isAttacking)
            {
                AttackCooldown();
                (CurrentActiveWeapon as IWeapon).Attack();
            }
        }
    }
    private void FindNearestEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, 
            (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponRange + PlayerController.Instance.otherStats.attackRange, LayerMask );
        if (enemiesInRange.Length == 0)
        {
            _moveFollow.enabled = true;
            return;
        }
        _moveFollow.enabled = false;
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;
        foreach (Collider2D enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }
        Vector2 direction = nearestEnemy.position - transform.position;
        // Tính toán góc giữa súng và mục tiêu
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        RotateGunTowardsTarget();
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, angle));
        if (angleDifference < 5f && !isAttacking)
        {
            (CurrentActiveWeapon as IWeapon).Attack();
            AttackCooldown();
        }
        void RotateGunTowardsTarget()
        {
            // Tạo hiệu ứng xoay mượt
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
        }
        
    }
}
