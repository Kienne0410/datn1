using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIKeyTouch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject OnTab;
    private UIControls _uiControls;

    private void Awake()
    {
        _uiControls = new UIControls();
    }

    private void OnEnable()
    {
        _uiControls.Enable();
    }

    private void OnDisable()
    {
        _uiControls.Disable();
    }

    void Start()

    {
        _uiControls.UI.OnTab.performed += _ =>
        {
            PauseGame(!OnTab.activeSelf);
            OnTab.SetActive(!OnTab.activeSelf);
        };
    }

    private void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
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
