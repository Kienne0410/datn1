using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    public void SetState(IState newState)
    {
        // Thoát khỏi state hiện tại
        _currentState?.OnExit();

        // Chuyển sang state mới
        _currentState = newState;

        // Gọi vào state mới
        _currentState?.OnEnter();
    }

    public void Update()
    {
        // Cập nhật state hiện tại
        _currentState?.OnUpdate();
    }
}

public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit(); 
}
