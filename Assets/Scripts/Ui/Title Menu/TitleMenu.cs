using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour {

    [SerializeField] string _startScene;
    [SerializeField] string _creditsScene;
    [SerializeField] float _sceneTransitionDelay = 1f;

    public void StartGame() {
        if (_startScene != null) {
            StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _startScene));
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void Credits() {
        if (_creditsScene != null) {
            StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _creditsScene));
        }
        else {
            Application.Quit();
        }
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
