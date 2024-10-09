using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] float _interactionRotationSpeed = 5f;
    [SerializeField] Transform _attachedGear;

    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _scriptedPhysics = GetComponent<PlayerScriptedPhysics>();
        _playerManager = GetComponent<PlayerManager>();
    }

    void Update() {

        if (_physics != null && Input.GetKeyDown(KeyCode.Space)) { _physics.Jump(); }

        if (Input.GetKeyDown(KeyCode.E)) { Interact(); }

        if (_playerManager.m_isInteracting == false) Move();

    }

    void FixedUpdate() {

        if (_playerManager.m_isInteracting == false) Rotate();

    }

    void Move() {

        float inputX = Input.GetAxis("Horizontal");
        Vector3 deplacement = new Vector3(inputX * _moveSpeed * Time.deltaTime, 0, 0);
        transform.Translate(deplacement, Space.World);

    }

    void Rotate() {

        float inputRotation = Input.GetAxis("Horizontal");
        float rotation = inputRotation * _rotationSpeed;
        if (rotation > 0) _playerManager.m_isTurningClockwise = true;
        else _playerManager.m_isTurningClockwise = false;

        _attachedGear.Rotate(Vector3.forward, -rotation);

    }

    void Interact() {

        _playerManager.m_isInteracting = !_playerManager.m_isInteracting;

        if (_playerManager.m_isInteracting) {

            _physics.m_playerMainCollider.isTrigger = true;
            _physics.m_playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            _attachedGear.Rotate(Vector3.forward, _interactionRotationSpeed);

        }

        else {

            _physics.m_playerMainCollider.isTrigger = false;
            _physics.m_playerRigidbody.constraints = RigidbodyConstraints2D.None;

        }
    }

    
}
