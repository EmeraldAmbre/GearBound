using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] Transform _attachedGear;

    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _scriptedPhysics = GetComponent<PlayerScriptedPhysics>();
        _playerManager = GetComponent<PlayerManager>();
    }

    void FixedUpdate() {

        if (_physics != null && Input.GetKeyDown(KeyCode.Space)) { _physics.Jump(); }

        Move(); Rotate();
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

    
}
