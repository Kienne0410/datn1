using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private Animator _areaExitAnimator;
    [SerializeField] private Collider2D _collider2D;

    private float waitToLoadTime = 1f;

    private void Awake()
    {
        if(_collider2D)
            _collider2D.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.score > 10)
        {
            if(_collider2D)
                _collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
            if(_areaExitAnimator)
                _areaExitAnimator.SetTrigger("Open");
        }
    }

    private IEnumerator LoadSceneRoutine() {
        while (waitToLoadTime >= 0) 
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
