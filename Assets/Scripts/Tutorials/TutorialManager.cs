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
        while (_currentStepIndex < tutorialSteps.Length)
        {
            ShowCurrentStep();
            yield return new WaitUntil(() => _tutorialPanel.activeSelf == false);
            yield return TutorialStep.Step(tutorialSteps[_currentStepIndex].completionQuest);
            yield return new WaitUntil(() => tutorialSteps[_currentStepIndex].QuestIsPassed());
            _currentStepIndex++;
        }
        Debug.Log("Tutorial Completed!");
        DataSerializer.Save(SaveKey.PassTutorial, true);
        StartCoroutine(FadeToLoadNextScene());
    }
    void Start()
    {
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
        GameManager.Instance.PauseGame(true);
        _tutorialPanel.SetActive(true);
        tutorialSteps[_currentStepIndex].uiElement.SetActive(true);
    }

    private IEnumerator FadeToLoadNextScene()
    {
        yield return UIFade.Instance.FadeRoutine(1);
        SceneManager.LoadScene("scene1");
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
