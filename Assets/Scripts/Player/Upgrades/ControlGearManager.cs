using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ControlGearManager : MonoBehaviour {

    // Allow gears to spin or not, by detecting where's the player and if it can interact
    [Header("Général settings")]
    [SerializeField] bool _isReversingEffectOnMechanism = false;
    [SerializeField] float _detectionRay = 1f; // Set a value big enough in editor (something near 1f ~ 1.2f)
    [SerializeField] LayerMask _detectionLayer; // Always set to "player" layer in editor
    [SerializeField] GameObject _playerPackage; // Drag and drop the package player here

    // Linked pulleys and linked interactions
    // Drag and drop your linked item(s) in editor
    [Header("Linked Items")]
    [SerializeField] RoomSpinner _linkedRoomToSpin;

    bool _isPlayerNear;
    int _precedentRotation;
    Rigidbody2D _gearRigidbody;

    void Start() {
        _gearRigidbody = GetComponent<Rigidbody2D>();
        _precedentRotation = (int)_gearRigidbody.rotation;
        _playerPackage = GameObject.FindWithTag("PlayerPackage");
    }

    void Update() {
        Collider2D[] objetsDetectes = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes.Length == 0) _gearRigidbody.freezeRotation = true;
        else _gearRigidbody.freezeRotation = false;

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