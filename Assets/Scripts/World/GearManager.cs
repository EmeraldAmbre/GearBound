using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    [SerializeField] float _detectionRay = 0.64f;
    [SerializeField] LayerMask _layerPlayer;
    [SerializeField] HingeJoint2D _hingeJoint;
    [SerializeField] GameObject _player;
    
    PlayerManager _playerManager;
    Rigidbody2D _gearRigidbody;

    void Start() {

        _hingeJoint = GetComponent<HingeJoint2D>();
        _playerManager = _player.GetComponent<PlayerManager>();
        _gearRigidbody = GetComponent<Rigidbody2D>();
        
    }

    void Update() {

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _layerPlayer);

        if (detectedObjects.Length > 0) _playerManager.m_isInteracting = true;

        else _playerManager.m_isInteracting = false;

        if (_playerManager.m_isInteracting) _hingeJoint.useMotor = true;

        else _hingeJoint.useMotor = false;

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }
}