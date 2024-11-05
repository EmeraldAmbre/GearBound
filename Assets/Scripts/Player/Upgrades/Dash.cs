using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour {

    static int _id = 0;
    static string _name = "dash";
    bool _isDashTextIsActive = false;

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
            _isDashTextIsActive = true;
            _textBox.SetActive(true);
        }
    }

    void OnDestroy() {
        _input.Player.CloseBoxText.performed -= OnPerformCloseText;
        _input.Player.Disable();
    }

    void OnPerformCloseText(InputAction.CallbackContext context) {
        if (_isDashTextIsActive) {
            _textBox.SetActive(false);
            gameObject.SetActive(false);
            _isDashTextIsActive = false;
        }
    }
}
