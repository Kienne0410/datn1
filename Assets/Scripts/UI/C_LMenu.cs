using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using ToolBox.Serialization;

public class C_LMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _loadGamePanel;

    [SerializeField] private Button _backButtonOptionPanel;
    [SerializeField] private Button _backButtonLoadGamePanel;
    
    [SerializeField] private OptionPanelControl _optionPanelControl;
    // Start is called before the first frame update
    private void PlayNewGameLoadScene()
    {
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        yield return UIFade.Instance.FadeRoutine(1);
        if (DataSerializer.Load<bool>(SaveKey.PassTutorial))
        {
            SceneManager.LoadScene("scene1");
        }
        else
        {
            SceneManager.LoadScene("scene2 1");
        }
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
        _optionPanelControl._musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        _optionPanelControl._sfxVolumeSlider.onValueChanged.AddListener(UpdateSfxVolume);
        _optionsPanel.SetActive(false);
        _loadGamePanel.SetActive(false);
        _playButton.onClick.AddListener(PlayNewGameLoadScene);
        _loadGameButton.onClick.AddListener(() =>SetActiveLoadGamePanel(true));
        _optionsButton.onClick.AddListener(() =>SetActiveOptionPanel(true));
        _backButtonOptionPanel.onClick.AddListener(CloseAndSaveOptionPanel);
        _backButtonLoadGamePanel.onClick.AddListener(() =>SetActiveLoadGamePanel(false));
    }

    private void CloseAndSaveOptionPanel()
    {
        _optionsPanel.SetActive(false);
        DataSerializer.Save(SaveKey.MusicVolume, SoundManager.Instance.musicVolume);
        DataSerializer.Save(SaveKey.SFXVolume, SoundManager.Instance.sfxVolume);
    }
    private void UpdateMusicVolume(float value)
    {
        SoundManager.Instance.UpdateMusicVolume(value);
    }

    private void UpdateSfxVolume(float value)
    {
        SoundManager.Instance.UpdateSfxVolume(value);
    }
    
    private void OnDestroy()
    {
        _optionPanelControl._musicVolumeSlider.onValueChanged.RemoveAllListeners();
        _optionPanelControl._sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
        _loadGameButton.onClick.RemoveAllListeners();
        _optionsButton.onClick.RemoveAllListeners();
        _backButtonOptionPanel.onClick.RemoveAllListeners();
        _backButtonLoadGamePanel.onClick.RemoveAllListeners();
    }
}
