using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Magnet : MonoBehaviour {

    static int _id = 2;
    static string _name = "magnet";
    bool _isMagnetTextIsActive = false;
    [SerializeField] AudioClip _sfxUpgrade;

    PlayerInputAction _input;

    [SerializeField] GameObject _textBox;

    public int ID { get { return _id; } private set { _id = value; } }
    public string Name { get { return _name; } private set { _name = value; } }

    void InitInput() {
        _input = new();
        _input.Player.CloseBoxText.performed += OnPerformCloseText;
        _input.Enable();
    }

    void Start() {
        _textBox.SetActive(false);
        InitInput();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerPrefs.SetInt(_name, 1);
            PlayerPrefs.Save();
            _isMagnetTextIsActive = true;
            _textBox.SetActive(true);
            AudioManager.Instance.PlaySfx(_sfxUpgrade, 9);
        }
    }

    void OnDestroy() {
        _input.Player.CloseBoxText.performed -= OnPerformCloseText;
        _input.Player.Disable();
    }

    void OnPerformCloseText(InputAction.CallbackContext context) {
        if (_isMagnetTextIsActive) {
            _textBox.SetActive(false);
            gameObject.SetActive(false);
            _isMagnetTextIsActive = false;
        }
    }
}
