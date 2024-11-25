using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour {

    [SerializeField] string _startScene;
    [SerializeField] string _creditsScene;
    [SerializeField] float _sceneTransitionDelay = 1f;
    [SerializeField] AudioClip _musicScene;
    [SerializeField] float fadeInAudioTime;
    [SerializeField] Animation _anim;
    [SerializeField] GameObject _transitionImage;
    // Start is called before the first frame update

    private async void Awake()
    {
        _anim.Play("ScreenBlacktransitionOut");

    }
    private void Update()
    {
        if (_anim.isPlaying == false && _transitionImage.activeSelf )
        {
            _transitionImage.SetActive(false);
        }
    }

    void Start()
    {
        if(!AudioManager.Instance.m_isMusicPlaying)
        {
            AudioManager.Instance.PlayMusic(_musicScene, fadeInAudioTime);
        }
    }
    public void StartGame() {
        if (_startScene != null) {

            _transitionImage.SetActive(true);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            AudioManager.Instance.StopMusic(2);
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

        _anim.Play("ScreenBlacktransitionIN");
        AudioManager.Instance.StopMusic(delay);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
