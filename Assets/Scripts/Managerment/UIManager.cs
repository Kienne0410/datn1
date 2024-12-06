using System;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        EventManager<GameEvent>.SubscribeAction(GameEvent.OnScoreIncrease, (Action) OnScoreIncrease);
    }

    private void OnScoreIncrease()
    {
        _scoreText.text = "Score : " + GameManager.Instance.score.ToString();
    }
    private void OnDestroy()
    {
        EventManager<GameEvent>.UnsubscribeAction(GameEvent.OnScoreIncrease,(Action) OnScoreIncrease);
    }
}
