using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class KingCrabBossEnemyAI : EnemyAI
{
    [SerializeField] private Transform[] polygonPoints;
    private int _normalAttackCount = 0;
    private float _attackAnimLength;
    private WaitForSeconds _animWait;
    private IState _currentIState;
    [SerializeField] private AudioClip _bossFightClip;
    protected override void Start()
    {
        _playerTransform = PlayerController.Instance.transform;
        roamPosition = GenerateRandomPointInPolygon();
        _attackAnimLength = GetTimeOfAttackAnim();
        _animWait = new WaitForSeconds(_attackAnimLength);
        SetState(new RoamingState(this));
    }

    public void SetState(IState newState)
    {
        _currentIState.OnExit();
        _currentIState = newState;
        _currentIState.OnEnter();
    }
    public float GetTimeOfAttackAnim()
    {
        AnimationClip attack = GetAnimationClip(_animator, "Attack");
        return attack.length;
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
    protected override void Update()
    {
        float _distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        switch (_currentState)
        {
            case State.Roaming:
                Roaming();
                if (IsPointInPolygon(_playerTransform.position))
                {
                    SoundManager.Instance.PlayBackgroundMusic(_bossFightClip);
                    _currentState = State.Chase;
                    _animator.SetTrigger("Chase");
                }
                break;
            case State.Dodge:
                if (canAttack)
                {
                    _currentState = State.Chase;
                    _animator.SetTrigger("Chase");
                }
                break;
            case State.Chase:
                ChasePlayer();
                if (_distanceToPlayer <= _attackRange)
                {
                    _currentState = State.Attack;
                }
                break;
            case State.Attack:
                if (_attackRange != 0 && canAttack && _normalAttackCount<3)
                {
                    _normalAttackCount++;
                    StartCoroutine(nameof(NormalAttackStep));
                }
                else
                {
                    _currentState = State.DropBom;
                }
                break;
            case State.DropBom:
                if (_attackRange != 0 && canAttack)
                {
                    StartCoroutine(nameof(DropBomAttackStep));
                }
                break;
                
        }
        
    }

    private void Dodge()
    {
        Vector3 randomOffset = Random.insideUnitCircle * _attackRange;
        Vector3 _dodgePos =randomOffset + _playerTransform.position;
        Vector2 direction = (_dodgePos - transform.position).normalized;
        _enemyPathfinding.MoveTo(direction, _speedRoaming);
    }
    
    private IEnumerator DropBomAttackStep()
    {
        _normalAttackCount = 0;
        _enemyPathfinding.StopMoving();
        _animator.SetTrigger("DropBom");
        canAttack = false;
        yield return _animWait;
        StartCoroutine(nameof(AttackCooldownRoutine));
        (_enemyType as KingCrabNormalAttack).DropBom();
        _currentState = State.Dodge;
        Dodge();
        _animator.SetTrigger("Roaming");
    }

    private IEnumerator NormalAttackStep()
    {
        _enemyPathfinding.StopMoving();
        _animator.SetTrigger("Attack");
        canAttack = false;
        yield return _animWait;
        StartCoroutine(nameof(AttackCooldownRoutine));
        (_enemyType as IEnemy).Attack();
        _currentState = State.Dodge;
        Dodge();
        _animator.SetTrigger("Roaming");

    }

    private Vector2 GenerateRandomPointInPolygon()
    {
        // Xác định phạm vi bao quanh đa giác (bounding box)
        float minX = Mathf.Min(polygonPoints[0].position.x, polygonPoints[1].position.x, polygonPoints[2].position.x, polygonPoints[3].position.x);
        float maxX = Mathf.Max(polygonPoints[0].position.x, polygonPoints[1].position.x, polygonPoints[2].position.x, polygonPoints[3].position.x);
        float minY = Mathf.Min(polygonPoints[0].position.y, polygonPoints[1].position.y, polygonPoints[2].position.y, polygonPoints[3].position.y);
        float maxY = Mathf.Max(polygonPoints[0].position.y, polygonPoints[1].position.y, polygonPoints[2].position.y, polygonPoints[3].position.y);

        // Sinh vị trí ngẫu nhiên trong bounding box
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
    void SetNewTargetPosition()
    {
        // Sinh vị trí mục tiêu ngẫu nhiên trong đa giác
        do
        {
            roamPosition = GenerateRandomPointInPolygon();
        } while (!IsPointInPolygon(roamPosition));
    }
    bool IsPointInPolygon(Vector2 point)
    {
        // Thuật toán Ray-Casting để kiểm tra xem điểm có trong đa giác không
        int intersections = 0;
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            Vector2 p1 = polygonPoints[i].position;
            Vector2 p2 = polygonPoints[(i + 1) % polygonPoints.Length].position;

            if ((point.y > Mathf.Min(p1.y, p2.y) && point.y <= Mathf.Max(p1.y, p2.y)) &&
                (point.x <= Mathf.Max(p1.x, p2.x)) &&
                (p1.y != p2.y))
            {
                float xinters = (point.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
                if (xinters > point.x)
                {
                    intersections++;
                }
            }
        }
        return (intersections % 2) != 0;
    }

    protected override void Roaming()
    {
        Vector2 bossRoamingDirection = (roamPosition - (Vector2)transform.position).normalized;
        _enemyPathfinding.MoveTo(bossRoamingDirection, _speedRoaming);
        if (Vector2.Distance(transform.position, roamPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }
    
}
