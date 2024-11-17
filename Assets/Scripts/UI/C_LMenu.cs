using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class C_LMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _loadGamePanel;

    [SerializeField] private Button _backButtonOptionPanel;
    [SerializeField] private Button _backButtonLoadGamePanel;
    // Start is called before the first frame update
    private void PlayNewGameLoadScene()
    {
        SceneManager.LoadScene("Scenes/scene1");
    }

    private void SetActiveOptionPanel(bool active)
    {
        _optionsPanel.SetActive(active);
    }

    private void SetActiveLoadGamePanel(bool active)
    {
        _loadGamePanel.SetActive(active);
    }
    void Awake()
    {
        _optionsPanel.SetActive(false);
        _loadGamePanel.SetActive(false);
        _playButton.onClick.AddListener(PlayNewGameLoadScene);
        _loadGameButton.onClick.AddListener(() =>SetActiveLoadGamePanel(true));
        _optionsButton.onClick.AddListener(() =>SetActiveOptionPanel(true));
        _backButtonOptionPanel.onClick.AddListener(() =>SetActiveOptionPanel(false));
        _backButtonLoadGamePanel.onClick.AddListener(() =>SetActiveLoadGamePanel(false));
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _loadGameButton.onClick.RemoveAllListeners();
        _optionsButton.onClick.RemoveAllListeners();
        _backButtonOptionPanel.onClick.RemoveAllListeners();
        _backButtonLoadGamePanel.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
