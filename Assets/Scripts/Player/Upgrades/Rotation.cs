using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotation : MonoBehaviour {

    static int _id = 1;
    static string _name = "rotation";
    bool _isRotationTextIsActive = false;

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
            _isRotationTextIsActive = true;
            _textBox.SetActive(true);
        }
    }

    void OnDestroy() {
        _input.Player.CloseBoxText.performed -= OnPerformCloseText;
        _input.Player.Disable();
    }

    void OnPerformCloseText(InputAction.CallbackContext context) {
        if (_isRotationTextIsActive) {
            _textBox.SetActive(false);
            gameObject.SetActive(false);
            _isRotationTextIsActive = false;
        }
    }
}
