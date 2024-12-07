using UnityEngine;

public class DodgeState : IState
{
    private readonly EnemyAI _context;

    public DodgeState(EnemyAI context)
    {
        _context = context;
    }

    public void OnEnter()
    {
        
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
        
    }
}
