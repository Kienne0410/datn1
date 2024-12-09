using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIKeyTouch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject OnTab;
    private UIControls _uiControls;

    private void OnEnable()
    {
        _uiControls = InputManager.Instance.uiControls;
        _uiControls.Enable();
    }

    private void OnDisable()
    {
        _uiControls.Disable();
    }

    void Start()
    {
        _uiControls.UI.OnTab.performed += _ => DisplayOnTab();
    }

    public void DisplayOnTab()
    {
        GameManager.Instance.PauseGameNotPauseUIControl(!OnTab.activeSelf);
        OnTab.SetActive(!OnTab.activeSelf);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

// Update is called once per frame
    void Update()
    {
        
    }
}
