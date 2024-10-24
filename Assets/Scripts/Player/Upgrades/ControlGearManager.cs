using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ControlGearManager : MonoBehaviour {

    [SerializeField] float _testRotation;

    bool _isPlayerNear;
    bool _isSwitched;
    float _precedentRotation;
    float _referenceRotation;

    PlayerController _controller;
    PlayerUpgrades _upgrades;
    Rigidbody2D _gearRigidbody;
    
    [SerializeField] GameObject _player;

    Vector3 _initialPosition;

    void Start() {
        _initialPosition = transform.position;
        _gearRigidbody = GetComponent<Rigidbody2D>();
        _precedentRotation = _gearRigidbody.rotation;
        _isSwitched = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = true;
            _controller = other.gameObject.GetComponent<PlayerController>();
            _upgrades = other.gameObject.GetComponent<PlayerUpgrades>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            _isPlayerNear = false;
        }
    }

    void Update() {

        if (_isPlayerNear == true && _controller != null) {

            if (_controller.m_inputX != 0 && _upgrades.m_canPossess && _referenceRotation < _testRotation) {

                _gearRigidbody.freezeRotation = false;

                _referenceRotation += _precedentRotation;

            }

            else if (_controller.m_inputX != 0 && _upgrades.m_canPossess && _referenceRotation >= _testRotation) {

                _gearRigidbody.freezeRotation = false;

                if (_isSwitched == false) {

                    SwitchPositionsAndSprites(gameObject, _player);

                    _isSwitched = true;

                }
            }
        }

        else {
            
            _gearRigidbody.freezeRotation = true;
        
        }

        if (_isSwitched == true && _upgrades.m_canPossess == false) {

            SwitchPositionsAndSprites(gameObject, _player);

            gameObject.transform.position = _initialPosition;

            _testRotation = 0;

            _isSwitched = false;
        
        }

        _precedentRotation = _gearRigidbody.rotation;

    }

    void SwitchPositionsAndSprites(GameObject objectA, GameObject objectB) {

        Vector3 tempPosition = objectA.transform.position;
        objectA.transform.position = objectB.transform.position;
        objectB.transform.position = tempPosition;

        SpriteRenderer spriteRendererA = objectA.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRendererB = objectB.GetComponent<SpriteRenderer>();

        if (spriteRendererA != null && spriteRendererB != null) {
            Sprite tempSprite = spriteRendererA.sprite;
            spriteRendererA.sprite = spriteRendererB.sprite;
            spriteRendererB.sprite = tempSprite;
        }
    }

}