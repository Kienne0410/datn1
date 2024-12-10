using System;
using System.Collections;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    public TutorialStep[] tutorialSteps; // Danh sách các bước hướng dẫn
    private int _currentStepIndex = 0; // Bước hiện tại
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Button _tutorialStepsPanelButton;
    public int GetCurrentStepIndex()
    {
        return _currentStepIndex;
    }
    protected override void Awake()
    {
        base.Awake();
        _tutorialStepsPanelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseGame(false);
            _tutorialPanel.SetActive(false);
        });
        Init();
    }

    private void OnDestroy()
    {
        _tutorialStepsPanelButton.onClick.RemoveAllListeners();
    }

    private IEnumerator StartTutorialSteps()
    {
        yield return new WaitUntil(() => _tutorialPanel.activeSelf == false);
        yield return TutorialStep.Step(tutorialSteps[_currentStepIndex].completionQuest);
        yield return new WaitUntil(() => tutorialSteps[_currentStepIndex].QuestIsPassed());
        _currentStepIndex++;
        ShowCurrentStep();
        StartCoroutine(nameof(StartTutorialSteps));
    }
    void Start()
    {
        ShowCurrentStep();
        StartCoroutine(nameof(StartTutorialSteps));
    }

    void Init()
    {
        // Ẩn tất cả UI bật cái Quest đầu tiên lên
        foreach (var step in tutorialSteps)
            step.uiElement.SetActive(false);
        tutorialSteps[0].uiElement.SetActive(true);
    }
    void ShowCurrentStep()
    {

        // Hiển thị UI của bước hiện tại
        if (_currentStepIndex < tutorialSteps.Length)
        {
            GameManager.Instance.PauseGame(true);
            _tutorialPanel.SetActive(true);
            tutorialSteps[_currentStepIndex].uiElement.SetActive(true);
        }
        else
        {
            StopCoroutine(nameof(StartTutorialSteps));
            Debug.Log("Tutorial Completed!");
            DataSerializer.Save(SaveKey.PassTutorial, true);
            SceneManager.LoadScene("scene1");
        }
    }

    public void CompleteCurrentStep(TutorialQuest condition)
    {
        // Kiểm tra điều kiện hoàn thành
        if (tutorialSteps[_currentStepIndex].completionQuest == condition)
        {
            tutorialSteps[_currentStepIndex].uiElement.SetActive(false);
            tutorialSteps[_currentStepIndex].Pass();
        }
    }
}
