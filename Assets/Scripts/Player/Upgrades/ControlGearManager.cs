using UnityEngine;

public class ControlGearManager : MonoBehaviour {
    
    bool _isPlayerNear;
    GameObject _player;
    Rigidbody2D _gearRigidbody;
    PlayerController _playerController;

    void Start() {
        _player = GameObject.FindWithTag("Player");
        _gearRigidbody = GetComponent<Rigidbody2D>();
        _playerController = _player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = false;
        }
    }

    void Update() {
        if (_isPlayerNear == true && _playerController != null) {
            if (_playerController.m_inputX != 0) {
                _gearRigidbody.freezeRotation = false;
            }
        }

        else {
            _gearRigidbody.freezeRotation = true;
        }
    }
}