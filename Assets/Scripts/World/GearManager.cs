using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    [SerializeField] bool _isInteractable;
    [SerializeField] float _detectionAngularRotation = 10f;
    [SerializeField] float _detectionRay = 1f;
    [SerializeField] LayerMask _detectionLayer = 7;

    // Linked pulleys and linked interactions
    [SerializeField] PulleySystem _linkedPulley;
    [SerializeField] SpinPulleySystem _linkedSpinPulley;
    [SerializeField] DrawbridgeSystem _linkedDrawbridge;
    [SerializeField] HorizontalPulleySystem _linkedHorizontalPulley;

    bool _isPlayerNear;
    bool _isInInteraction;

    Rigidbody2D _gearRigidbody;

    void Start() {
        _gearRigidbody = GetComponent<Rigidbody2D>();

        _isInInteraction = false;
    }

    void Update() {

        Collider2D[] objetsDetectes = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes.Length == 0 && _isInteractable) _gearRigidbody.freezeRotation = true;
        else _gearRigidbody.freezeRotation = false;

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

        // Horizontal Pulley System
        if (_linkedHorizontalPulley != null) {
            if (_gearRigidbody.angularVelocity > _detectionAngularRotation) {
                _linkedHorizontalPulley.m_isMovingLeft = true;
                _linkedHorizontalPulley.m_isMovingRight = false;
            }

            else if (_gearRigidbody.angularVelocity < -_detectionAngularRotation) {
                _linkedHorizontalPulley.m_isMovingLeft = false;
                _linkedHorizontalPulley.m_isMovingRight = true;
            }

            else {
                _linkedHorizontalPulley.m_isMovingLeft = false;
                _linkedHorizontalPulley.m_isMovingRight = false;
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

        // Drawbridge System
        if (_linkedDrawbridge != null) {
            if (_gearRigidbody.angularVelocity > _detectionAngularRotation) {
                _linkedDrawbridge.m_isMovingDown = true;
                _linkedDrawbridge.m_isMovingUp = false;
            }

            else if (_gearRigidbody.angularVelocity < -_detectionAngularRotation) {
                _linkedDrawbridge.m_isMovingDown = false;
                _linkedDrawbridge.m_isMovingUp = true;
            }

            else {
                _linkedDrawbridge.m_isMovingDown = false;
                _linkedDrawbridge.m_isMovingUp = false;
            }
        }
    }
}