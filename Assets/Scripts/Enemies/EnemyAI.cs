using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats")] 
    [SerializeField]
    private float _speedRoaming = 2f;

    [SerializeField] private float _speedChasing = 4f;
    [SerializeField] private float _speedAttacking = 5f;
    [SerializeField] protected float _attackRange = 2f;
    [SerializeField] protected float _fieldOfViewToChase = 10f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] protected MonoBehaviour _enemyType;
    [SerializeField] private float _roamChangeDirFloat = 2f;
    [SerializeField] private bool _stopMovingWhileAttacking = false;
    protected State _currentState = State.Roaming;
    [Header("Other")] [SerializeField] protected Animator _animator;
    protected Transform _playerTransform;
    [SerializeField] protected EnemyPathfinding _enemyPathfinding;


    protected bool canAttack = true;

    protected enum State {
        Roaming,
        Chase,
        Attack
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;
    

    private void Start() {
        _playerTransform = PlayerController.Instance.transform;
        roamPosition = GetRoamingPosition();
    }

    private void Update() 
    {
        float _distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        switch (_currentState)
        {
            case State.Roaming:
                Roaming();
                if (_distanceToPlayer <= _fieldOfViewToChase)
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
                    _animator.SetTrigger("Attack");
                }
                else if (_distanceToPlayer > _fieldOfViewToChase)
                {
                    _currentState = State.Roaming;
                    _animator.SetTrigger("Roaming");
                }

                break;
            case State.Attack:
                //AttackPlayer();
                if (_distanceToPlayer > _attackRange)
                {
                    _currentState = State.Chase;
                    _animator.SetTrigger("Chase");
                }
                break;
        }
        
    }

    protected void Roaming() {
        timeRoaming += Time.deltaTime;

        _enemyPathfinding.MoveTo(roamPosition, _speedRoaming);

        if (timeRoaming > _roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    protected void ChasePlayer()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _enemyPathfinding.MoveTo(direction, _speedChasing);
    }

    protected virtual void AttackPlayer() {

        if (_attackRange != 0 && canAttack) {

            canAttack = false;
            (_enemyType as IEnemy).Attack();

            if (_stopMovingWhileAttacking) {
                _enemyPathfinding.StopMoving();
            } else {
                _enemyPathfinding.MoveTo(roamPosition, _speedAttacking);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    protected IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(_attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
