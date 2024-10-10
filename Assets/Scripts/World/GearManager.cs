using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    [SerializeField] float _detectionRay = 0.70f;
    [SerializeField] float _detectionAngularRotation = 10f;

    [SerializeField] LayerMask _layerPlayer;
    [SerializeField] HingeJoint2D _playerJoint;
    [SerializeField] GameObject _player;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Rigidbody2D _playerRigidbody;

    [SerializeField] Vector3 _interactionPosition;
    [SerializeField] PulleySystem _linkedPulley;
    [SerializeField] SpinPulleySystem _linkedSpinPulley;

    bool _isPlayerNear;
    bool _isInInteraction;

    Rigidbody2D _gearRigidbody;

    void Start() {

        if (_playerJoint == null) _playerJoint = _player.GetComponent<HingeJoint2D>();
        if (_playerManager == null) _playerManager = _player.GetComponent<PlayerManager>();
        if (_playerRigidbody == null) _playerRigidbody = _player.GetComponent<Rigidbody2D>();

        _gearRigidbody = GetComponent<Rigidbody2D>();

        _isInInteraction = false;
    }

    void Update() {

        Collider2D[] detectedObj = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _layerPlayer);

        if (detectedObj.Length > 0) _isPlayerNear = true;
        else _isPlayerNear = false;

        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E) && !_isInInteraction) {
            BeginInteraction();
        }

        if (_isInInteraction && Input.GetKeyDown(KeyCode.F)) {
            EndInteraction();
        }

        // Pulley System
        if (_linkedPulley != null) {
            if (_gearRigidbody.angularVelocity > _detectionAngularRotation) {
                _linkedPulley.m_isMovingDown = true;
                _linkedPulley.m_isMovingUp = false;
            }

            else if (_gearRigidbody.angularVelocity < -_detectionAngularRotation) {
                _linkedPulley.m_isMovingDown = false;
                _linkedPulley.m_isMovingUp = true;
            }

            else {
                _linkedPulley.m_isMovingDown = false;
                _linkedPulley.m_isMovingUp = false;
            }
        }

        // Spinning Pulley System
        if (_linkedSpinPulley != null) {
            if (_gearRigidbody.angularVelocity > _detectionAngularRotation) {
                _linkedSpinPulley.m_isSpinningLeft = true;
                _linkedSpinPulley.m_isSpinningRight = false;
            }

            else if (_gearRigidbody.angularVelocity < -_detectionAngularRotation) {
                _linkedSpinPulley.m_isSpinningLeft = false;
                _linkedSpinPulley.m_isSpinningRight = true;
            }

            else {
                _linkedSpinPulley.m_isSpinningLeft = false;
                _linkedSpinPulley.m_isSpinningRight = false;
            }
        }
    }

    void BeginInteraction() {

        _isInInteraction = true;
        _playerManager.m_isInteracting = true;
        _player.transform.position = _interactionPosition;
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _playerJoint.enabled = true;
        _playerJoint.useMotor = true;

    }

    void EndInteraction() {

        _isInInteraction = false;
        _playerManager.m_isInteracting = false;
        _playerJoint.useMotor = false;
        _playerJoint.enabled = false;
        _gearRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _gearRigidbody.constraints = RigidbodyConstraints2D.None;
        _gearRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerRigidbody.constraints = RigidbodyConstraints2D.None;

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }

}