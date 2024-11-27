using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private GameObject _OnTabElement;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    
    private void Awake()
    {
        _yesButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        _noButton.onClick.AddListener( () =>
        {
            gameObject.SetActive(!gameObject.activeSelf);
            _OnTabElement.SetActive(true);
        });
    }

    private void OnDestroy()
    {
        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();
    }
}
