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

    [Header("Controls Background")]
    [SerializeField] Image _controlsBackgroundImage1;
    [SerializeField] Image _controlsBackgroundImage2;
    [SerializeField] Image _controlsBackgroundImage3;
    [SerializeField] Image _controlsBackgroundImage4;
    [SerializeField] Image _controlsBackgroundImage5;

    [Header("Upgrades Images")]
    [SerializeField] Image _imageUpgrade1;
    [SerializeField] Image _imageUpgrade2;
    [SerializeField] Image _imageUpgrade3;
    [SerializeField] Image _imageUpgrade4;

    [Header("Locked Upgrades Images")]
    [SerializeField] Image _imageLockedUpgrade1;
    [SerializeField] Image _imageLockedUpgrade2;
    [SerializeField] Image _imageLockedUpgrade3;
    [SerializeField] Image _imageLockedUpgrade4;

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

            _titleText.enabled = true;

            int index = 0;

            if (PlayerPrefs.GetInt("dash") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("magnet") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("rotation") == 1) { index += 1; }
            if (PlayerPrefs.GetInt("possession") == 1) { index += 1; }

            switch (index) {

                case 0:
                    _controlsBackgroundImage1.enabled = true;
                    _imageLockedUpgrade1.enabled = true;
                    _imageLockedUpgrade2.enabled = true;
                    _imageLockedUpgrade3.enabled = true;
                    _imageLockedUpgrade4.enabled = true;
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
                    _controlsBackgroundImage2.enabled = true;
                    _imageUpgrade1.enabled = true;
                    _imageLockedUpgrade2.enabled = true;
                    _imageLockedUpgrade3.enabled = true;
                    _imageLockedUpgrade4.enabled = true;
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
                    _controlsBackgroundImage3.enabled = true;
                    _imageUpgrade1.enabled = true;
                    _imageUpgrade2.enabled = true;
                    _imageLockedUpgrade3.enabled = true;
                    _imageLockedUpgrade4.enabled = true;
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
                    _controlsBackgroundImage4.enabled = true;
                    _imageUpgrade1.enabled = true;
                    _imageUpgrade2.enabled = true;
                    _imageUpgrade3.enabled = true;
                    _imageLockedUpgrade4.enabled = true;
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
                    _controlsBackgroundImage5.enabled = true;
                    _imageUpgrade1.enabled = true;
                    _imageUpgrade2.enabled = true;
                    _imageUpgrade3.enabled = true;
                    _imageUpgrade4.enabled = true;
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
            _isPaused = false;
        }
    }

    void DesactivateUI() {
        _titleText.enabled = false;

        _controlsBackgroundImage1.enabled = false;
        _controlsBackgroundImage2.enabled = false;
        _controlsBackgroundImage3.enabled = false;
        _controlsBackgroundImage4.enabled = false;
        _controlsBackgroundImage5.enabled = false;

        _imageUpgrade1.enabled = false;
        _imageUpgrade2.enabled = false;
        _imageUpgrade3.enabled = false;
        _imageUpgrade4.enabled = false;

        _imageLockedUpgrade1.enabled = false;
        _imageLockedUpgrade2.enabled = false;
        _imageLockedUpgrade3.enabled = false;
        _imageLockedUpgrade4.enabled = false;

        _imageText1.enabled = false;
        _imageText2.enabled = false;
        _imageText3.enabled = false;
        _imageText4.enabled = false;
    }
}
