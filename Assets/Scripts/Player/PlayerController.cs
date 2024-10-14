using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Movement
    bool _isSpinningFixed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] Transform _attachedGear;
    
    // Physics
    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;
    Rigidbody2D _rigidbody;

    float inputX;
    bool _isJumping = false;

    private void Awake() {
        _isSpinningFixed = false;
    }

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _scriptedPhysics = GetComponent<PlayerScriptedPhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && _isSpinningFixed == false && _physics.m_isGrounded)
        {
            _isSpinningFixed = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        else if (Input.GetKeyDown(KeyCode.E) && _isSpinningFixed == true)
        {
            _isSpinningFixed = false;
            _rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        if (_physics != null && Input.GetKeyDown(KeyCode.Space))
        {
            if (_physics.m_isGrounded) _isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.F)) _playerManager.RotationInversion();

        inputX = Input.GetAxis("Horizontal");
    }

    float _gravity = 9.81f;
    float _yGravityAcelerationToApply = 0;

    void FixedUpdate() {

        if (_playerManager.m_isInteracting == false) Rotate();

        Vector2 newVelocity = new Vector3(inputX * _moveSpeed * Time.deltaTime, _rigidbody.velocity.y, 0);
        if (_physics.m_isGrounded) newVelocity.y = 0;
        if (_isJumping)
        {
            _rigidbody.AddForce(new Vector2(0 ,_physics._jumpForce), ForceMode2D.Impulse);
            _isJumping = false;
        }

        _rigidbody.velocity += newVelocity;

        _attachedGear.transform.position = transform.position;
    }

    void Rotate() {

        float inputRotation = Input.GetAxis("Horizontal");
        float rotation = inputRotation * _rotationSpeed * _playerManager.m_rotationInversion;
        _attachedGear.Rotate(Vector3.forward, -rotation);

    }


  

}
