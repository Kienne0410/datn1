using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private void Start()
    {
        EventManager.OnScoreIncrease += OnScoreIncrease;
    }

    private void OnScoreIncrease()
    { 
        _scoreText.text = "Score : " + GameManager.Instance.score.ToString();
    }
    private void OnDestroy()
    {
        EventManager.OnScoreIncrease -= OnScoreIncrease;
    }
}
