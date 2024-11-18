using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour {

    [SerializeField] string _mainMenuScene;
    [SerializeField] float _sceneTransitionDelay = 1f;

    [Header("Websites")]
    [SerializeField] string _itchioUrl = "https://ambre-emeraude.itch.io/";

    public void MainMenu() {
        if (_mainMenuScene != null) {
            StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _mainMenuScene));
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void OpenItchio() {
        Application.OpenURL(_itchioUrl);
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
