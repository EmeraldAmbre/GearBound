using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Movement
    bool _isSpinningFixed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] float _interactionRotationSpeed = 5f;
    [SerializeField] Transform _attachedGear;

    // Physics
    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;
    Rigidbody2D _rigidbody;

    // Player Joint
    [SerializeField] HingeJoint2D _joint;

    // Gears
    [SerializeField] LayerMask _layerGear;
    [SerializeField] float _gearDetectionRay = 1f;

    private void Awake() {
        _isSpinningFixed = false;
    }

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _scriptedPhysics = GetComponent<PlayerScriptedPhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _joint = GetComponent<HingeJoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {

        //Collider2D[] detectedGears = Physics2D.OverlapCircleAll(transform.position, _gearDetectionRay, _layerGear);
        //if (detectedGears.Length == 0 && Input.GetKeyDown(KeyCode.E) && _isSpinningFixed == false) BeginFixedRotation();
        //else if (Input.GetKeyDown(KeyCode.F) && _isSpinningFixed == true) EndFixedRotation();

        if (_physics != null && Input.GetKeyDown(KeyCode.Space)) {
            if (_physics.m_isGrounded) _physics.Jump();
        }

        if (_playerManager.m_isInteracting == false) Move();

        if (Input.GetKeyDown(KeyCode.P)) _playerManager.RotationInversion(_joint);

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
        _attachedGear.Rotate(Vector3.forward, -rotation);

    }

    void BeginFixedRotation() {
        _isSpinningFixed = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _joint.enabled = true;
        _joint.useMotor = true;
    }

    void EndFixedRotation() {
        _isSpinningFixed = false;
        _joint.useMotor = false;
        _joint.enabled = false;
        _rigidbody.freezeRotation = true;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

}
