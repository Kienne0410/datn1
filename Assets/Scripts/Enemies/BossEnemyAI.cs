using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyAI : EnemyAI
{
    protected override void Update() 
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
                
                AttackPlayer();
                if (_distanceToPlayer > _attackRange)
                {
                    _currentState = State.Chase;
                    _animator.SetTrigger("Chase");
                }
                break;
        }
        
    }
    protected override void AttackPlayer() {
    
        if (_attackRange != 0 && canAttack) {
            _enemyPathfinding.StopMoving();
            canAttack = false;  
            (_enemyType as IEnemy).Attack();
            
            // if (_stopMovingWhileAttacking) {
            //     _enemyPathfinding.StopMoving();
            // } else {
            //     _enemyPathfinding.MoveTo(roamPosition, _speedAttacking);
            // }
    
            StartCoroutine(AttackCooldownRoutine());
        }
    }
}
