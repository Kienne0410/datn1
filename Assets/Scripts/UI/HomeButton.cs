using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _OnTabElement;
    [SerializeField] private GameObject _HomeTab;
     private void Awake()
    {
        _button.onClick.AddListener(()=>
        {
            _OnTabElement.SetActive(!_OnTabElement.activeSelf);
            _HomeTab.SetActive(!_HomeTab.activeSelf);
        });
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
