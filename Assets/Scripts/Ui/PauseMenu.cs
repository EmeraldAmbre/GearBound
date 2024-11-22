using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    void Start() {
        InitInput();
        DesactivateUI();
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

    void OnPerformPause(InputAction.CallbackContext context) {

        if (_isPaused is false) {

            DesactivateUI();

            Time.timeScale = 0f;

            _titleText.enabled = true;
            _background.SetActive(true);

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
                    _imageText1.text = "Dash";
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
                    _imageText1.text = "Dash";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize";
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
                    _imageText1.text = "Dash";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize";
                    _imageText3.enabled = true;
                    _imageText3.text = "Inverse Rotation";
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
                    _imageText1.text = "Dash";
                    _imageText2.enabled = true;
                    _imageText2.text = "Magnetize";
                    _imageText3.enabled = true;
                    _imageText3.text = "Inverse Rotation";
                    _imageText4.enabled = true;
                    _imageText4.text = "Possession";
                    break;
            }
            _isPaused = true;
        }

        else if (_isPaused is true) {
            DesactivateUI();
            Time.timeScale = 1f;
            _isPaused = false;
        }
    }

    void DesactivateUI() {
        _titleText.enabled = false;
        _background.SetActive(false);

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
