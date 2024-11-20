using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Singleton<HealthBar>
{
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private TextMeshProUGUI _healthBarText;
    
    public void SetHeathBarText(string health)
    {
        _healthBarText.text = health;
    }

    public void SetSliderHealthBarValue(float ratio)
    {
        _healthBarSlider.value = ratio;
    }
    
}
