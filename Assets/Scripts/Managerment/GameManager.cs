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
        EventManager<GameEvent>.SubscribeAction(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
    }
    
    

    private void OnDestroy()
    {
        // K cần vì k bị destroy đi, nhưng viết cho đúng bản chất quản lí Event
        EventManager<GameEvent>.SubscribeAction(GameEvent.OnScoreIncrease, (Action)OnScoreIncrease);
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

    public bool GetIsPaused()
    {
        return isPaused;
    }
    
}
