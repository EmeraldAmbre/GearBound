using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    [SerializeField] float _linearMotorAcceleration = 0.25f;
    [SerializeField] float _minMotorSpeed = 2.5f;
    [SerializeField] float _maxMotorSpeed = 100f;
    [SerializeField] float _detectionRay = 0.70f;
    [SerializeField] LayerMask _layerPlayer;
    [SerializeField] HingeJoint2D _hingeJoint;
    [SerializeField] GameObject _player;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Rigidbody2D _gearRigidbody;

    bool _isPlayerNear;

    void Start() {

        _hingeJoint = GetComponent<HingeJoint2D>();
        if (_playerManager == null) _playerManager = _player.GetComponent<PlayerManager>();
        if (_gearRigidbody == null) _gearRigidbody = GetComponent<Rigidbody2D>();
        
    }

    void Awake() {

        _gearRigidbody.freezeRotation = false;
        
    }

    void Update() {

        Collider2D[] detectedObj = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _layerPlayer);

        if (detectedObj.Length > 0) _isPlayerNear = true;
        else _isPlayerNear = false;

        if (_isPlayerNear && _playerManager.m_isInteracting) Activate();
        else Desactivate();

    }

    void Activate() {

        _hingeJoint.useMotor = true;

    }

    void Desactivate() {

        _hingeJoint.useMotor = false;

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }

}