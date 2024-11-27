using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Possession : MonoBehaviour {

    static int _id = 3;
    static string _name = "possession";
    bool _isPossessionTextIsActive = false;
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
            _isPossessionTextIsActive = true;
            _textBox.SetActive(true);
            AudioManager.Instance.PlaySfx(_sfxUpgrade, 9);
            FindFirstObjectByType<PlayerController>().m_isControllable = false;
        }
    }

    void OnDestroy() {
        _input.Player.CloseBoxText.performed -= OnPerformCloseText;
        _input.Player.Disable();
    }

    void OnPerformCloseText(InputAction.CallbackContext context)
    {
        FindFirstObjectByType<PlayerController>().m_isControllable = true;
        if (_isPossessionTextIsActive) {
            _textBox.SetActive(false);
            gameObject.SetActive(false);
            _isPossessionTextIsActive = false;
        }
    }
}
