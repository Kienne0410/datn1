using UnityEngine;

public class RoamingState : IState
{
    private readonly EnemyAI _context;

    public RoamingState(EnemyAI context)
    {
        _context = context;
    }

    public void OnEnter()
    {
        Debug.Log("Entering Idle State");
        _context._animator.SetTrigger("Idle");
    }

    public void OnUpdate()
    {
        // if (_context.DistanceToPlayer <= _context.FieldOfView)
        // {
        //     _context.StateMachine.SetState(new ChaseState(_context));
        // }
    }

    public void OnExit()
    {
        Debug.Log("Exiting Idle State");
    }
}
