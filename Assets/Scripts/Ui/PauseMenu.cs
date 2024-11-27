using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    bool _isPaused = false;

    PlayerInputAction _input;

    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] GameObject _background;

    [Header("Controls Background")]
    [SerializeField] GameObject _controlsBackgroundImage1;
    [SerializeField] GameObject _controlsBackgroundImage2;
    [SerializeField] GameObject _controlsBackgroundImage3;
    [SerializeField] GameObject _controlsBackgroundImage4;
    [SerializeField] GameObject _controlsBackgroundImage5;

    [Header("Upgrades Images")]
    [SerializeField] GameObject _imageUpgrade1;
    [SerializeField] GameObject _imageUpgrade2;
    [SerializeField] GameObject _imageUpgrade3;
    [SerializeField] GameObject _imageUpgrade4;

    [Header("Locked Upgrades Images")]
    [SerializeField] GameObject _imageLockedUpgrade1;
    [SerializeField] GameObject _imageLockedUpgrade2;
    [SerializeField] GameObject _imageLockedUpgrade3;
    [SerializeField] GameObject _imageLockedUpgrade4;

    [Header("Upgrades Text")]
    [SerializeField] TextMeshProUGUI _imageText1;
    [SerializeField] TextMeshProUGUI _imageText2;
    [SerializeField] TextMeshProUGUI _imageText3;
    [SerializeField] TextMeshProUGUI _imageText4;


    [Header("SFX")]
    [SerializeField] AudioClip _sfxMenuOpen;
    [SerializeField] AudioClip _sfxMenuClosed;

    [Header("VFX")]
    [SerializeField] GameObject _transitionScreen;

    [Header("Buttons")]
    [SerializeField] string _mainMenuScene;
    [SerializeField] float _sceneTransitionDelay = 1f;
    [SerializeField] GameObject _mainMenuButton;
    [SerializeField] GameObject _exitButton;
    [SerializeField] Button _mainMenuBt;

    void Start() {
        InitInput();
        DesactivateUI();
        Cursor.visible = false;
    }

    void OnDestroy() {
        _input.Player.Pause.performed -= OnPerformPause;
        _input.Player.Disable();
    }

    void InitInput() {
        _input = new();
        _input.Player.Pause.performed += OnPerformPause;
        _input.Enable();
    }

    public void ExitGame() {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void MainMenu() {
        if (_mainMenuScene != null)
        {
            FindAnyObjectByType<PlayerController>().m_isControllable = false;
            StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _mainMenuScene));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    
    void OnPerformPause(InputAction.CallbackContext context) {

        if (_isPaused is false) {
            _transitionScreen.SetActive(false);
            AudioManager.Instance.PlaySfx(_sfxMenuOpen, 6);
            DesactivateUI();

            FindAnyObjectByType<PlayerController>().m_isControllable = false;


            _titleText.enabled = true;
            _background.SetActive(true);
            _mainMenuButton.SetActive(true);
            _exitButton.SetActive(true);

            _mainMenuBt.Select();

            int index = 0;

            if (PlayerPrefs.GetInt("dash") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("magnet") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("rotation") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("possession") == 1) { index += 1; }

            switch (index) {

                case 0:
                    _controlsBackgroundImage1.SetActive(true);
                    _imageLockedUpgrade1.SetActive(true);
                    _imageLockedUpgrade2.SetActive(true);
                    _imageLockedUpgrade3.SetActive(true);
                    _imageLockedUpgrade4.SetActive(true);
                    _imageText1.enabled = true;
                    _imageText1.text = "Locked";
                    _imageText2.enabled = true;
                    _imageText2.text = "Locked";
                    _imageText3.enabled = true;
                    _imageText3.text = "Locked";
                    _imageText4.enabled = true;
                    _imageText4.text = "Locked";
                    break;

                case 1:
                    _controlsBackgroundImage2.SetActive(true);
                    _imageUpgrade1.SetActive(true);
                    _imageLockedUpgrade2.SetActive(true);
                    _imageLockedUpgrade3.SetActive(true);
                    _imageLockedUpgrade4.SetActive(true);
                    _imageText1.enabled = true;
                    _imageText1.text = "Dash (Shift)";
                    _imageText2.enabled = true;
                    _imageText2.text = "Locked";
                    _imageText3.enabled = true;
                    _imageText3.text = "Locked";
                    _imageText4.enabled = true;
                    _imageText4.text = "Locked";
                    break;

                case 2:
                    _controlsBackgroundImage3.SetActive(true);
                    _imageUpgrade1.SetActive(true);
                    _imageUpgrade2.SetActive(true);
                    _imageLockedUpgrade3.SetActive(true);
                    _imageLockedUpgrade4.SetActive(true);
                    _imageText1.enabled = true;
                    _imageText1.text = "Dash (Shift)";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize (Z)";
                    _imageText3.enabled = true;
                    _imageText3.text = "Locked";
                    _imageText4.enabled = true;
                    _imageText4.text = "Locked";
                    break;

                case 3:
                    _controlsBackgroundImage4.SetActive(true);
                    _imageUpgrade1.SetActive(true);
                    _imageUpgrade2.SetActive(true);
                    _imageUpgrade3.SetActive(true);
                    _imageLockedUpgrade4.SetActive(true);
                    _imageText1.enabled = true;
                    _imageText1.text = "Dash (Shift)";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize (Z)";
                    _imageText3.enabled = true;
                    _imageText3.text = "Inverse Rotation (S)";
                    _imageText4.enabled = true;
                    _imageText4.text = "Locked";
                    break;

                case 4:
                    _controlsBackgroundImage5.SetActive(true);
                    _imageUpgrade1.SetActive(true);
                    _imageUpgrade2.SetActive(true);
                    _imageUpgrade3.SetActive(true);
                    _imageUpgrade4.SetActive(true);
                    _imageText1.enabled = true;
                    _imageText1.text = "Dash (Shift)";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize (Z)";
                    _imageText3.enabled = true;
                    _imageText3.text = "Inverse Rotation (S)";
                    _imageText4.enabled = true;
                    _imageText4.text = "Possession (A)";
                    break;
            }
            _isPaused = true;

            Cursor.visible = true;
        }

        else if (_isPaused is true)
        {
            Cursor.visible = false;
            _transitionScreen.SetActive(true);

            AudioManager.Instance.PlaySfx(_sfxMenuClosed, 6);
            DesactivateUI();
            FindAnyObjectByType<PlayerController>().m_isControllable = true;

            _isPaused = false;
        }
    }

    void DesactivateUI() {
        _titleText.enabled = false;
        _background.SetActive(false);
        _mainMenuButton.SetActive(false);
        _exitButton.SetActive(false);

        _controlsBackgroundImage1.SetActive(false);
        _controlsBackgroundImage2.SetActive(false);
        _controlsBackgroundImage3.SetActive(false);
        _controlsBackgroundImage4.SetActive(false);
        _controlsBackgroundImage5.SetActive(false);

        _imageUpgrade1.SetActive(false);
        _imageUpgrade2.SetActive(false);
        _imageUpgrade3.SetActive(false);
        _imageUpgrade4.SetActive(false);

        _imageLockedUpgrade1.SetActive(false);
        _imageLockedUpgrade2.SetActive(false);
        _imageLockedUpgrade3.SetActive(false);
        _imageLockedUpgrade4.SetActive(false);

        _imageText1.enabled = false;
        _imageText2.enabled = false;
        _imageText3.enabled = false;
        _imageText4.enabled = false;
    }


}
