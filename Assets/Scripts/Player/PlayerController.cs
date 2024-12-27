using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private CharacterDefaultStatsData characterDefaultStatsData; 
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform _slashSpawnPoint;
    public int _currentLevel { get; set; } = 1;
    public int _currentExp { get; set; } = 0; 
    public float _currentHealth { get; set; }
    public CharacterDefaultStats _defaultStats { get; set; }
    public CharacterOtherStats _otherStats { get; set; } = new CharacterOtherStats();
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    
    private Action<InputAction.CallbackContext> dashAction;

    protected override void Awake() {
        base.Awake();
        playerControls = InputManager.Instance.playerControls;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        dashAction = _ => Dash();
        playerControls.Combat.Dash.performed += dashAction;
        InitStats();
    }

    public void InitStats()
    {
        _defaultStats = characterDefaultStatsData.characterStats[_currentLevel-1];
        _currentHealth = _defaultStats.maxHealth;
        startingMoveSpeed = _defaultStats.moveSpeed;
    }

    public void LevelUp()
    {
        _currentLevel++;
        _defaultStats = characterDefaultStatsData.characterStats[_currentLevel-1];
        startingMoveSpeed = _defaultStats.moveSpeed;
    }

    public float GetTimeOfDieAnim()
    {
        AnimationClip die = GetAnimationClip(myAnimator, "die");
        return die.length;
    }
    private AnimationClip GetAnimationClip(Animator animator, string clipName)
    {
        // Tìm animation clip theo tên
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }

    private void OnDestroy()
    {
        playerControls.Combat.Dash.performed -= dashAction;
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider() {
        return weaponCollider;
    }

    public Transform GetSlashSpawnPoint()
    {
        return _slashSpawnPoint;
    }
    public SpriteRenderer GetSpriteRenderer()
    {
        return _playerSpriteRenderer;
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    public void PlayerDie()
    {
        myAnimator.SetTrigger("die");
    }
    private void Move() {
        if (knockback.GettingKnockedBack) { return; }

        rb.MovePosition(rb.position + movement * (startingMoveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash() {
        if (!isDashing) {
            isDashing = true;
            startingMoveSpeed *= _defaultStats.dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        startingMoveSpeed = _defaultStats.moveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
