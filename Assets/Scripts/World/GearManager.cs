using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    // Allow gears to spin or not, by detecting where's the player and if it can interact
    [Header("Non optionnal settings")]
    [SerializeField] bool _isInteractable; // Check it in editor if u want that this gear can spin with player
    [SerializeField] bool _isReversingEffectOnMechanism = false;
    [SerializeField] float _detectionRay = 1f; // Set a value big enough in editor (something near 1f ~ 1.2f)
    [SerializeField] LayerMask _detectionLayer; // Always set to "player" layer in editor

    // Linked pulleys and linked interactions
    // Drag and drop your linked item(s) in editor
    [Header("Linked Items")]
    [SerializeField] PulleySystem _linkedPulley;
    [SerializeField] SpinPulleySystem _linkedSpinPulley;
    [SerializeField] DrawbridgeSystem _linkedDrawbridge;
    [SerializeField] HorizontalPulleySystem _linkedHorizontalPulley;
    [SerializeField] RoomSpinner _linkedRoomToSpin;

    bool _isPlayerNear;
    int _precedentRotation;
    Rigidbody2D _gearRigidbody;

    void Start() {
        _gearRigidbody = GetComponent<Rigidbody2D>();
        _precedentRotation = (int)_gearRigidbody.rotation;
    }

    void Update() {
        Collider2D[] objetsDetectes = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes.Length == 0 && _isInteractable) _gearRigidbody.freezeRotation = true;
        else _gearRigidbody.freezeRotation = false;

        // Pulley System
        if (_linkedPulley != null) {
            if (_precedentRotation < (int)_gearRigidbody.rotation) {
                _linkedPulley.m_isMovingDown = !_isReversingEffectOnMechanism;
                _linkedPulley.m_isMovingUp = _isReversingEffectOnMechanism;
            }

            else if (_precedentRotation > (int)_gearRigidbody.rotation) {
                _linkedPulley.m_isMovingDown = _isReversingEffectOnMechanism;
                _linkedPulley.m_isMovingUp = !_isReversingEffectOnMechanism;
            }

            else {
                _linkedPulley.m_isMovingDown = false;
                _linkedPulley.m_isMovingUp = false;
            }
        }

        // Horizontal Pulley System
        if (_linkedHorizontalPulley != null) {
            if (_precedentRotation < (int)_gearRigidbody.rotation) {
                _linkedHorizontalPulley.m_isMovingLeft = !_isReversingEffectOnMechanism;
                _linkedHorizontalPulley.m_isMovingRight = _isReversingEffectOnMechanism;
            }

            else if (_precedentRotation > (int)_gearRigidbody.rotation) {
                _linkedHorizontalPulley.m_isMovingLeft = _isReversingEffectOnMechanism;
                _linkedHorizontalPulley.m_isMovingRight = !_isReversingEffectOnMechanism;
            }

            else {
                _linkedHorizontalPulley.m_isMovingLeft = false;
                _linkedHorizontalPulley.m_isMovingRight = false;
            }
        }

        // Spinning Pulley System
        if (_linkedSpinPulley != null) {
            if (_precedentRotation < (int)_gearRigidbody.rotation) {
                _linkedSpinPulley.m_isSpinningLeft = !_isReversingEffectOnMechanism;
                _linkedSpinPulley.m_isSpinningRight = _isReversingEffectOnMechanism;
            }

            else if (_precedentRotation > (int)_gearRigidbody.rotation) {
                _linkedSpinPulley.m_isSpinningLeft = _isReversingEffectOnMechanism;
                _linkedSpinPulley.m_isSpinningRight = !_isReversingEffectOnMechanism;
            }

            else {
                _linkedSpinPulley.m_isSpinningLeft = false;
                _linkedSpinPulley.m_isSpinningRight = false;
            }
        }

        // Drawbridge System
        if (_linkedDrawbridge != null) {
            if (_precedentRotation < (int)_gearRigidbody.rotation) {
                _linkedDrawbridge.m_isMovingDown = !_isReversingEffectOnMechanism;
                _linkedDrawbridge.m_isMovingUp = _isReversingEffectOnMechanism;
            }

            else if (_precedentRotation > (int)_gearRigidbody.rotation) {
                _linkedDrawbridge.m_isMovingDown = !_isReversingEffectOnMechanism;
                _linkedDrawbridge.m_isMovingUp = _isReversingEffectOnMechanism;
            }

            else {
                _linkedDrawbridge.m_isMovingDown = false;
                _linkedDrawbridge.m_isMovingUp = false;
            }
        }

        // Spin the Room !
        if (_linkedRoomToSpin != null) {
            if (_precedentRotation < (int)_gearRigidbody.rotation) {
                _linkedRoomToSpin.m_isSpinningLeft = !_isReversingEffectOnMechanism;
                _linkedRoomToSpin.m_isSpinningRight = _isReversingEffectOnMechanism;
            }
            else if (_precedentRotation > (int)_gearRigidbody.rotation) {
                _linkedRoomToSpin.m_isSpinningLeft = _isReversingEffectOnMechanism;
                _linkedRoomToSpin.m_isSpinningRight = !_isReversingEffectOnMechanism;
            }
            else {
                _linkedRoomToSpin.m_isSpinningLeft = false;
                _linkedRoomToSpin.m_isSpinningRight = false;
            }
        }

        _precedentRotation = (int)_gearRigidbody.rotation;

    }
}