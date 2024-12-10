using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelControl : MonoBehaviour
{
    public Slider _musicVolumeSlider;
    public Slider _sfxVolumeSlider;

    private void OnEnable()
    {
        _musicVolumeSlider.value = SoundManager.Instance.musicVolume;
        _sfxVolumeSlider.value = SoundManager.Instance.sfxVolume;
    }
}
