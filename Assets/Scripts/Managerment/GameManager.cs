using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    private bool isPaused = false;
    public int score = 0;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action<int>)OnScoreIncrease);
        EventManager.Subscribe(GameEvent.GetIsPaused, (Func<bool>) GetIsPaused);
    }
    
    
    

    private void OnDestroy()
    {
        // K cần vì k bị destroy đi, nhưng viết cho đúng bản chất quản lí Event
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease, (Action<int>)OnScoreIncrease);
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
        EventManager.Unsubscribe(GameEvent.GetIsPaused, (Func<bool>) GetIsPaused);
    }

    public void PauseGame(bool status)
    {
        isPaused = status;
        Time.timeScale = status ? 0 : 1;
    }

    private void OnScoreIncrease()
    {
        score++;
    }
    private void OnScoreIncrease(int addScore)
    {
        score = score + addScore;
    }

    private bool GetIsPaused()
    {
        return isPaused;
    }
    
}
