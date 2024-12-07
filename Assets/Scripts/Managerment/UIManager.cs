using System;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action) OnScoreIncrease);
    }

    private void OnScoreIncrease()
    {
        _scoreText.text = "Score : " + GameManager.Instance.score.ToString();
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease,(Action) OnScoreIncrease);
    }
}
