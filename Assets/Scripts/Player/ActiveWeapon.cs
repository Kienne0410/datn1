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

    protected override void Awake() {
        base.Awake();
        playerControls = InputManager.Instance.playerControls;
        _StartAttacking = _ => StartAttacking();
        _StopAttacking = _ => StopAttacking();
        playerControls.Combat.Attack.started += _StartAttacking;
        playerControls.Combat.Attack.canceled += _StopAttacking;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls?.Disable();
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
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
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
        if (attackButtonDown && !isAttacking) {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
