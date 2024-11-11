using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class C_LMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    // Start is called before the first frame update
    private void PlayNewGameLoadScene()
    {
        SceneManager.LoadScene("Scenes/scene1");
    }
    void Awake()
    {
        _playButton.onClick.AddListener(PlayNewGameLoadScene);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
