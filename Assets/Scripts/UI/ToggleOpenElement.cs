using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOpenElement : MonoBehaviour
{
    [SerializeField] private GameObject _element;
    [SerializeField] private Toggle _toggle;

    private void Awake()
    {
        _toggle.onValueChanged.AddListener((isOn) => _element.SetActive(isOn));
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveAllListeners();
    }
}
