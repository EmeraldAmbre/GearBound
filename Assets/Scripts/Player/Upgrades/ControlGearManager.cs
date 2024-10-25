using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ControlGearManager : MonoBehaviour {

    [SerializeField] int _testRotation = 0;
    [SerializeField] Vector3 _targetPosition;

    bool _isPlayerNear;
    bool _isSwitched;
    int _referenceRotation;

    Rigidbody2D _gearRigidbody;
    SpriteRenderer _gearRenderer;

    [SerializeField] PlayerUpgrades _upgrades;
    [SerializeField] GameObject _playerPackage;
    [SerializeField] SpriteRenderer _playerRenderer;
    [SerializeField] PlayerController _playerController;

    Vector3 _initialPosition;

    void Start() {
        _initialPosition = transform.position;
        _gearRigidbody = GetComponent<Rigidbody2D>();
        _gearRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = false;
        }
    }

    void Update() {
        if (_isPlayerNear == true && _playerController != null) {
            if (_playerController.m_inputX != 0) {
                _gearRigidbody.freezeRotation = false;
                _referenceRotation += 1;
            }
        }

        else {
            _gearRigidbody.freezeRotation = true;
        }

        if (_upgrades != null) {
            if (_upgrades.m_canPossess == true && _isSwitched == false && _referenceRotation > _testRotation) {
                SwitchPositionsAndSprites();
                _isSwitched = true;
                _referenceRotation = 0;
                _gearRigidbody.freezeRotation = true;

            }
            else if (_upgrades.m_canPossess == true && _isSwitched == true) {
                transform.position = _initialPosition;
                SwitchPositionsAndSprites();
                _isSwitched = false;
            }
        }
    }

    void SwitchPositionsAndSprites() {

        Vector3 tempPosition = transform.position;
        transform.position = _playerPackage.transform.position;

        if (_targetPosition != Vector3.zero) {
            _playerPackage.transform.position = _targetPosition;
        }

        else {
            _playerPackage.transform.position = tempPosition;
        }

        if (_gearRenderer != null && _playerRenderer != null) {
            Sprite tempSprite = _gearRenderer.sprite;
            _gearRenderer.sprite = _playerRenderer.sprite;
            _playerRenderer.sprite = tempSprite;

            Color tempColor = _gearRenderer.color;
            _gearRenderer.color = _playerRenderer.color;
            _playerRenderer.color = tempColor;
        }
    }

}