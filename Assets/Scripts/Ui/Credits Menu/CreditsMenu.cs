using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour {

    [SerializeField] string _mainMenuScene;
    [SerializeField] float _sceneTransitionDelay = 1f;
    [SerializeField] TextMeshProUGUI _timer;
    [SerializeField] TextMeshProUGUI _hearts;

    [Header("Websites")]
    [SerializeField] string _itchioUrl = "https://ambre-emeraude.itch.io/";

    void Awake() {
        _timer.text = "Final Timer: " + PlayerPrefs.GetFloat("GameTime");
        _hearts.text = "Unlocked Hearts: " + (PlayerPrefs.GetInt("max_player_life")-1).ToString();
    }

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

    IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
