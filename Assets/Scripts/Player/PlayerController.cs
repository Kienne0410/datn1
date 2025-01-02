using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI _hpRegenText;
    [SerializeField] private RectTransform _hpRegenTextRectTransform;
    public int _currentLevel { get; set; } = 1;
    public int _currentExp { get; set; } = 0; 
    public float _currentHealth { get; set; }
    public CharacterDefaultStats defaultStats { get; set; }
    public CharacterOtherStats otherStats { get; set; } = new CharacterOtherStats();
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
        defaultStats = characterDefaultStatsData.characterStats[_currentLevel-1];
        _currentHealth = defaultStats.maxHealth;
        startingMoveSpeed = defaultStats.moveSpeed;
    }
    
    public void AddExp(int exp)
    {
        _currentExp += exp;
        EventManager.Raise(UIEvent.OnUpdateExpBar);
        if (_currentExp >= defaultStats.EXPtoNextLevel)
        {
            _currentExp = 0;
            LevelUp();
        }
    }
    public void LevelUp()
    {
        _currentLevel++;
        defaultStats = characterDefaultStatsData.characterStats[_currentLevel-1];
        startingMoveSpeed = defaultStats.moveSpeed;
        EventManager.Raise(GameEvent.OnUpdateLevel);
        EventManager.Raise(UIEvent.OnUpdateExpBar);
        EventManager.Raise(UIEvent.OnUpdateHealthBar);
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
        playerControls?.Disable();
        playerControls.Combat.Dash.performed -= dashAction;
    }

    private void Update() {
        PlayerInput();
        HealthRegen();
    }

    private float _regenTimer = 0f;
    private void HealthRegen()
    {
        _regenTimer += Time.deltaTime;

        if (_regenTimer >= 1f) // Nếu đủ 1 giây
        {
            _regenTimer = 0f; // Reset bộ đếm
            if (!(_currentHealth < defaultStats.maxHealth)) return;
            _currentHealth = Mathf.Clamp(_currentHealth + defaultStats.regenSpeed, 0, defaultStats.maxHealth);
            _hpRegenText.text = $"+{defaultStats.regenSpeed}HP";
            EventManager.Raise(UIEvent.OnUpdateHealthBar);
            StartCoroutine(ShowRegenEffect());
        }
    }
    private IEnumerator ShowRegenEffect()
    {
        float duration = 0.5f; // Thời gian fade
        float elapsedTime = 0f;
        Color originalColor = _hpRegenText.color;
        
        Vector3 originalPosition = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(40,40,0);

        // Tăng vị trí dọc trong quá trình fade
        Vector3 targetPosition = originalPosition + new Vector3(0, 30, 0);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            // Interpolate vị trí để nhảy lên
            _hpRegenTextRectTransform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);

            // Giảm alpha để fade dần
            Color fadedColor = originalColor;
            fadedColor.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            _hpRegenText.color = fadedColor;

            yield return null;
        }

        // Reset text sau khi fade
        _hpRegenText.color = originalColor;
        _hpRegenText.text = ""; // Ẩn text sau khi hoàn thành
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
            startingMoveSpeed *= defaultStats.dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine() {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        startingMoveSpeed = defaultStats.moveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
