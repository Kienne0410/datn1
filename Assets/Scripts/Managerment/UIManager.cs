using System;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Splines;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private TextMeshProUGUI _healthBarText;
    [SerializeField] private Slider _expBarSlider;
    [SerializeField] private TextMeshProUGUI _expBarText;
    [SerializeField] private TextMeshProUGUI _levelText;
    private void Awake()
    {
        OnScoreUpdate();
        OnUpdateExpBar();
        OnUpdateHealthBar();
    }

    private void OnUpdateHealthBar()
    {
        SetHealthBar(PlayerController.Instance._currentHealth,PlayerController.Instance._defaultStats.maxHealth);
    }

    private void OnUpdateExpBar()
    {
        SetEXPBar(PlayerController.Instance._currentExp,PlayerController.Instance._defaultStats.EXPtoNextLevel);
    }
    private void SetHealthBar(float currentHealth, float maxHealth)
    {
        _healthBarText.text = $"{currentHealth}/{maxHealth}";
        _healthBarSlider.value = currentHealth/maxHealth;
    }
    private void SetEXPBar(int currentEXP, int EXPtoNextLevel)
    {
        _expBarText.text = $"{currentEXP}/{EXPtoNextLevel}";
        _expBarSlider.value = (float)currentEXP/EXPtoNextLevel;
    }

    private void OnUpdateLevel()
    {
        _levelText.text = "Lv" + PlayerController.Instance._currentLevel;
    }
    private void Start()
    {
        EventManager.Subscribe(GameEvent.OnScoreIncrease, (Action) OnScoreUpdate);
        EventManager.Subscribe(UIEvent.OnUpdateExpBar, (Action) OnUpdateExpBar);
        EventManager.Subscribe(UIEvent.OnUpdateHealthBar, (Action) OnUpdateHealthBar);
        EventManager.Subscribe(GameEvent.OnUpdateLevel, (Action) OnUpdateLevel);
    }

    private void OnScoreUpdate()
    {
        _scoreText.text = "Score : " + GameManager.Instance.score;
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEvent.OnScoreIncrease,(Action) OnScoreUpdate);
        EventManager.Unsubscribe(UIEvent.OnUpdateExpBar, (Action) OnUpdateExpBar);
        EventManager.Unsubscribe(UIEvent.OnUpdateHealthBar, (Action) OnUpdateHealthBar);
        EventManager.Unsubscribe(GameEvent.OnUpdateLevel, (Action) OnUpdateLevel);
    }
}
