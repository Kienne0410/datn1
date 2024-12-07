using UnityEngine;

public class ChaseState
{
    private readonly EnemyAI _context;

    public ChaseState(EnemyAI context)
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
