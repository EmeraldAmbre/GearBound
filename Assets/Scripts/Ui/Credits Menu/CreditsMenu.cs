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
        float timer = PlayerPrefs.GetFloat("GameTime");
        int hours = Mathf.FloorToInt(timer / 3600);
        int minutes = Mathf.FloorToInt((timer % 3600) / 60);
        int secs = Mathf.FloorToInt(timer % 60);
        string timeFormat = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, secs);
        _timer.text = "Final Timer: " + timeFormat;
        _hearts.text = "Unlocked Hearts: " + ((PlayerPrefs.GetInt("max_player_life")/2)-1).ToString();
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
