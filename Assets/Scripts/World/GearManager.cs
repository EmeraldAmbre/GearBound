using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    [SerializeField] float _detectionRay = 0.70f;
    [SerializeField] float _interactionTimer = 4.0f;
    [SerializeField] Vector3 _targetAnchor;

    [SerializeField] LayerMask _layerPlayer;
    [SerializeField] HingeJoint2D _playerJoint;
    [SerializeField] GameObject _player;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Rigidbody2D _playerRigidbody;

    bool _isPlayerNear;
    bool _isInInteraction;
    float _timer;

    void Start() {

        if (_playerJoint == null) _playerJoint = _player.GetComponent<HingeJoint2D>();
        if (_playerManager == null) _playerManager = _player.GetComponent<PlayerManager>();
        if (_playerRigidbody == null) _playerRigidbody = _player.GetComponent<Rigidbody2D>();
        
    }

    void Awake() {

        _isInInteraction = false;
        
    }

    void Update() {

        Collider2D[] detectedObj = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _layerPlayer);

        if (detectedObj.Length > 0) _isPlayerNear = true;
        else _isPlayerNear = false;

        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E) && !_isInInteraction) {
            BeginInteraction();
        }

        if (_isInInteraction) {
            _timer -= Time.deltaTime;

            if (_timer > 0) {
                // eventual supplementary actions
            }

            else {
                EndInteraction();
            }
        }

    }

    void BeginInteraction() {

        _timer = _interactionTimer;
        _isInInteraction = true;
        _playerManager.m_isInteracting = true;
        _player.transform.position = _targetAnchor;
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _playerJoint.enabled = true;
        _playerJoint.useMotor = true;

    }

    void EndInteraction() {

        _isInInteraction = false;
        _playerManager.m_isInteracting = false;
        _playerJoint.useMotor = false;
        _playerJoint.enabled = false;
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerRigidbody.constraints = RigidbodyConstraints2D.None;

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }

}