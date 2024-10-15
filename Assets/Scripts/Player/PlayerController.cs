using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Movement
    bool _isSpinningFixed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] Transform _attachedGear;


    [SerializeField] float _jumpHandlingVelocity = 5;

    [SerializeField] float _jumpBufferTime = 0.13f;
    float _jumpBufferTimer = 5;

    [SerializeField] float _jumpCoyoteTime = 0.13f;
    float _jumpCoyoteTimer = 5;


    float _initGravityScale;
    [SerializeField] float _fallGravityMultiplicator = 2;


    [SerializeField] float _velocityXMax = 20000;
    [SerializeField] float _velocityJumpMax = 20000;
    [SerializeField] float _velocityFallMax = -20000;


    // Physics
    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;
    Rigidbody2D _rigidbody;

    float inputX;
    bool _isJumping = false;
    bool _isHandlingJumpButton = false;

    private void Awake() {
        _isSpinningFixed = false;
    }

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _scriptedPhysics = GetComponent<PlayerScriptedPhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _initGravityScale = _rigidbody.gravityScale;
    }

    void Update()
    {
        if (!_isJumping && !_physics.m_isGrounded && _rigidbody.velocity.y < 0 && _jumpCoyoteTimer == 0) _jumpCoyoteTimer = 0;
        else if (_jumpCoyoteTimer <= _jumpCoyoteTime) _jumpCoyoteTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && !_physics.m_isGrounded) _jumpBufferTimer = 0;
        else if (_jumpBufferTimer <= _jumpBufferTime) _jumpBufferTimer += Time.deltaTime;

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
        if (_physics != null && ( 
            (Input.GetKeyDown(KeyCode.Space) && _physics.m_isGrounded)
            || (_physics.m_isGrounded && _jumpBufferTimer <= _jumpBufferTime)
            || (Input.GetKeyDown(KeyCode.Space) && !_isJumping && !_physics.m_isGrounded && _jumpCoyoteTimer <= _jumpCoyoteTime)
            ))
        {
            _isJumping = true;
            if (_jumpCoyoteTimer <= _jumpCoyoteTime) Debug.Log("Coyote timer : " + _jumpCoyoteTimer);
            else if (_jumpBufferTimer <= _jumpBufferTime) Debug.Log("Jump buffering timer : " + _jumpBufferTimer);

        }
        if (_physics != null && Input.GetKey(KeyCode.Space))
        {
            if (_physics.m_isGrounded) _isHandlingJumpButton = true;
        }
        else _isHandlingJumpButton = false;

        if (Input.GetKeyDown(KeyCode.F)) _playerManager.RotationInversion();

        inputX = Input.GetAxis("Horizontal");
    }

    void FixedUpdate() {

        if (_playerManager.m_isInteracting == false) Rotate();
        
        if (_rigidbody.velocity.y < 0 && _rigidbody.gravityScale == _initGravityScale && !_physics.m_isGrounded) _rigidbody.gravityScale *= _fallGravityMultiplicator;
        else if (_rigidbody.velocity.y >= 0 && _rigidbody.gravityScale != _initGravityScale) _rigidbody.gravityScale = _initGravityScale;
        Vector2 newVelocity = new Vector3(inputX * _moveSpeed, _rigidbody.velocity.y, 0);
        //if (_physics.m_isGrounded) newVelocity.y = 0;
        if (_isJumping)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, 0);
            _rigidbody.AddForce(new Vector2(0 ,_physics._jumpForce), ForceMode2D.Impulse);
        }
        if (_isHandlingJumpButton && _rigidbody.velocity.y >= 0)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpHandlingVelocity), ForceMode2D.Force);
        }

        if (_physics.m_isGrounded && !Input.GetKeyDown(KeyCode.Space))
        {
            _isJumping = false;
        }

        _rigidbody.velocity += newVelocity;

        //_rigidbody.velocity = new Vector2(
        //    Mathf.Clamp(_rigidbody.velocity.x, -_velocityXMax, _velocityXMax)
        //    ,Mathf.Clamp(_rigidbody.velocity.y, _velocityFallMax, _velocityJumpMax)
        //);

        _attachedGear.transform.position = transform.position;
    }

    void Rotate() {

        float inputRotation = Input.GetAxis("Horizontal");
        float rotation = inputRotation * _rotationSpeed * _playerManager.m_rotationInversion;
        _attachedGear.Rotate(Vector3.forward, -rotation);
    }


  

}
