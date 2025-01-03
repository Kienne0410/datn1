using System;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        OnScoreUpdate();
    }

    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action) OnScoreUpdate);
    }

    private void OnScoreUpdate()
    {
        _scoreText.text = "Score : " + GameManager.Instance.score.ToString();
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease,(Action) OnScoreUpdate);
    }
}
