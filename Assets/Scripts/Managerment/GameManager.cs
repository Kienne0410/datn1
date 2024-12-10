using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    private bool isPaused = false;
    public int score = 0;
    public int playerHealth = 10;
    public int currentHealth;
    protected override void Awake()
    {
        currentHealth = playerHealth;
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
        EventManager.Subscribe(GameEvent.GetIsPaused, (Func<bool>) GetIsPaused);
    }
    
    
    private void OnDestroy()
    {
        // K cần vì k bị destroy đi, nhưng viết cho đúng bản chất quản lí Event
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
        EventManager.Unsubscribe(GameEvent.GetIsPaused, (Func<bool>) GetIsPaused);
    }

    public void PauseGame(bool status)
    {
        InputManager.Instance.PauseInputSystem(status);
        isPaused = status;
        if (!status)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void PauseGameNotPauseUIControl(bool status)
    {
        isPaused = status;
        if (!status)
        {   
            InputManager.Instance.playerControls.Enable();
            Time.timeScale = 1f;
        }
        else
        {
            InputManager.Instance.playerControls.Disable();
            Time.timeScale = 0f;
        }
    }

    private void OnScoreIncrease()
    {
        score++;
    }

    private bool GetIsPaused()
    {
        return isPaused;
    }
    
}
