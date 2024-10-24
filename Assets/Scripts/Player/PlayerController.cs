using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] float _initMoveSpeed = 50f;
    float _currentSpeed;
    [SerializeField] float _groundAcceleration = 7f;
    [SerializeField] float _groundDeceleration = 17f;
    [SerializeField] float _airAcceleration = 7f;
    [SerializeField] float _airDeceleration = 17f;
    [SerializeField] float _velocityXMax = 20000;
    [SerializeField] float _velocityYJumpMax = 20000;
    [SerializeField] float _velocityYFallMax = -150;

    [Header("Jump values")]
    [SerializeField] float _jumpForce = 7.5f;
    [SerializeField] float _jumpHandlingVelocity = 5;

    [SerializeField] float _jumpBufferTime = 0.13f;
    float _jumpBufferTimer = 5;
    bool _isCoyoteTimerStarted = false;
    [SerializeField] float _jumpCoyoteTime = 0.13f;
    float _jumpCoyoteTimer = 5;

    [Header("Air peak values")]
    [SerializeField] float _yVelocityPeakThreshold = 10f;
    [SerializeField] float _peakGravityMultiplicator = 0.8f;
    [SerializeField] float _peakXMovementMultiplicator = 1.2f;
    [SerializeField] float _peakAcceleration = 1.2f;
    [SerializeField] float _peakDeceleration = 1.2f;


    [Header("Physics")]
    Vector2 _velocity = Vector2.zero;
    [SerializeField] float _initGravity = 9.81f;
    float _currentGravity;
    [SerializeField] float _fallGravityMultiplicator = 2;
    [SerializeField] PhysicsMaterial2D _physicMaterialFullFriction;
    [SerializeField] PhysicsMaterial2D _physicMaterialZeroFriction;
    PlayerCompositePhysics _physics;
    PlayerManager _playerManager;
    Rigidbody2D _rigidbody;

    // Physics
    [Header("Gear")]
    [SerializeField] float _onGearSpeedMultiplicator = 0.7f;
    [SerializeField] float _onGearVelocityYCap = -3;
    [SerializeField] float _onGearWallSpeedMultiplicator = 5f;
    [SerializeField] float _onGearWallVelocityYCap = 0;
    [SerializeField] float _gearRotationSpeed = 20f;
    [SerializeField] float _onGearWallGearRotationSpeed = 300f;
    [SerializeField] float _onGearWallGearGravity = 0;
    [SerializeField] Vector2 _gearWallJumpForceVector = Vector2.zero;
    [SerializeField] float _airGearWallJumpAcceleration = 7f;
    [SerializeField] float _airGearWallJumpDeceleration = 17f;
    public bool m_isGearWallJumping = false;
    public float m_currentGearRotation { get; private set; } = 0;
    [SerializeField] GameObject _body;
    Quaternion _bodyInitialRotation;

    public float m_inputX { get; private set; } = 0;
    bool _isTrigerringJump = false;
    bool _hasJumped = false;
    bool _isHandlingJumpButton = false;

    PlayerInputAction _input;

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentGravity = _initGravity;
        _currentSpeed = _initMoveSpeed;
        InitInput();
        _bodyInitialRotation = _body.transform.rotation;
    }

    #region Input Methods

    private void InitInput()
    {
        _input = new();
        _input.Player.Jump.started += OnPerformJumpStarted;
        _input.Player.Jump.canceled += OnPerformJumpCanceled;
        _input.Player.Movement.performed += OnPerformXAxis;
        _input.Enable();
    }

    private void OnPerformXAxis(InputAction.CallbackContext context)
    {
        m_inputX = context.ReadValue<Vector2>().normalized.x;
    }

    private void OnPerformJumpCanceled(InputAction.CallbackContext context)
    {
        _isHandlingJumpButton = false;
    }

    private void OnPerformJumpStarted(InputAction.CallbackContext context)
    {
        if ((_physics.IsGrounded() || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall()) && !_hasJumped)
        {
            _isTrigerringJump = true;
            if (_physics.IsOnContactWithGearWall() && !_physics.IsGrounded()) m_isGearWallJumping = true;
        }
        if (!(_physics.IsGrounded() || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall()))
        {
            _jumpBufferTimer = 0;
        }
        if (_jumpCoyoteTimer < _jumpCoyoteTime)
        {
            _jumpCoyoteTimer = _jumpCoyoteTime;
            ResetYVelocityOfPlayerAndGearRigidbodies();
            _isTrigerringJump = true;
        }

        _isHandlingJumpButton = true;

    }

    private void OnDestroy()
    {
        _input.Player.Jump.started -= OnPerformJumpStarted;
        _input.Player.Jump.canceled -= OnPerformJumpCanceled;
        _input.Player.Movement.performed -= OnPerformXAxis;
        _input.Player.Disable();
    } 
    #endregion

    void Update()
    {
        HandlingCoyoteJump();

        HandleJumpBuffering();

        ResetNeededDataWhenOnGround();
    }


    #region Jump and movement methods called in Update()

    private void HandleJumpBuffering()
    {
        if (_jumpBufferTimer < _jumpBufferTime) _jumpBufferTimer += Time.deltaTime;
        if (_physics.IsGrounded() && _jumpBufferTimer < _jumpBufferTime)
        {
            _isTrigerringJump = true;
            _jumpBufferTimer = _jumpBufferTime;
        }
    }
    private void HandlingCoyoteJump()
    {
        if (!_hasJumped && !_physics.IsGrounded() && _velocity.y < 0 && !_isCoyoteTimerStarted)
        {
            _jumpCoyoteTimer = 0;
            _isCoyoteTimerStarted = true;
        }
        else if (_jumpCoyoteTimer < _jumpCoyoteTime) _jumpCoyoteTimer += Time.deltaTime;
   
    }

    private void ResetNeededDataWhenOnGround()
    {
        if ((_physics.IsGrounded() && _velocity.y <= 0.01f) || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall())
        {
            _hasJumped = false;
            _isCoyoteTimerStarted = false;
            if (!_physics.IsOnContactWithGearWall()) m_isGearWallJumping = false;
        }
    }
    #endregion


    void FixedUpdate() {
        // ORDER HAVE IMPORTANCE DON'T CHANGE THE ORDER UNLESS YOU KNOW WHAT YOU DO
        // ~ I never know what I'm doing .. :3
        //_rigidbody.velocity = Vector2.zero;


        HandleCheckSlopePhysicsMaterialReset();


        HandlePhysicsGravityChange();
        HandlePhysicsGravity();


        HandlePhysicsXMovement();

        HandlePhysicsVelocityWenJumpTriggered();
        HandlePhysicsVelocityWhenJumpHandling();


        HandleCheckCeilingVelocityReset();

        ClampVelocity();

        // _rigidbody.MovePosition((Vector2) transform.position + _velocity * Time.fixedDeltaTime);
        _rigidbody.velocity = _velocity;

        if(m_isGearWallJumping) Debug.Log("Velocity When jump wall gear : " + _velocity);
        UpdateGearRotation();


    }

    //private void LateUpdate()
    //{
    //    _body.transform.rotation = _bodyInitialRotation;
    //}

    #region Physics methods for FixedUpdate()
    private void HandlePhysicsXMovement() {
        // On ground
        if ((_physics.IsGrounded() && _velocity.y <= 0.01f))
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed, _groundDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed, _groundAcceleration );
        }
        // On jump peak
        else if (IsOnPeakThresholdJump())
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _peakXMovementMultiplicator, _peakDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _peakXMovementMultiplicator, _peakAcceleration);
        }
        // On gear
        else if (_physics.IsOnContactWithGear())
        {

            // _velocity.x = 0;
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _onGearSpeedMultiplicator, _groundDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _onGearSpeedMultiplicator, _groundAcceleration );
        }
        // On gear wall
        else if (_physics.IsOnContactWithGearWall() && !m_isGearWallJumping)
        {
            if (m_inputX == 0)
            {
                _velocity.x = 0;

            }
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _onGearWallSpeedMultiplicator, _groundAcceleration);    
        }
        // In air when gear wall jumping
        else if (m_isGearWallJumping)
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _onGearWallSpeedMultiplicator, _airGearWallJumpDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed * _onGearWallSpeedMultiplicator, _airGearWallJumpAcceleration);
        }
        // In air
        else
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed, _airDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * _currentSpeed, _airAcceleration);
        }
    }


    private void HandleCheckSlopePhysicsMaterialReset() {

        if (_physics.IsOnSlope()) {
            if (m_inputX == 0) _rigidbody.sharedMaterial = _physicMaterialFullFriction;
            else _rigidbody.sharedMaterial = _physicMaterialZeroFriction;
        }

        else _rigidbody.sharedMaterial = _physicMaterialZeroFriction;
    }



    private void UpdateGearRotation() {

        if (_playerManager.m_isInteracting == false) {

            if (m_inputX == 0) m_currentGearRotation = Mathf.Lerp(m_currentGearRotation, m_inputX * _gearRotationSpeed, _groundDeceleration);
            else if (_physics.IsOnContactWithGearWall()) m_currentGearRotation = Mathf.Lerp(m_currentGearRotation, m_inputX * _onGearWallGearRotationSpeed, _groundAcceleration);
            else m_currentGearRotation = Mathf.Lerp(m_currentGearRotation, m_inputX * _gearRotationSpeed, _groundAcceleration);

            transform.Rotate(Vector3.forward, - m_currentGearRotation);
        }

        _body.transform.rotation = _bodyInitialRotation;
    }

    private void HandleCheckCeilingVelocityReset()
    {
        if (_physics.IsCeiling() && _velocity.y > 0.1)
        {
            _velocity.y = 0;
            _isHandlingJumpButton = false;
        }
    }

    private void HandlePhysicsVelocityWhenJumpHandling()
    {
        if (_isHandlingJumpButton && _velocity.y > 0.01f && !_physics.IsOnContactWithGearWall())
        {
            _velocity += new Vector2(0, _jumpHandlingVelocity);
        }
    }

    private void HandlePhysicsVelocityWenJumpTriggered()
    {
        if (_isTrigerringJump)
        {
            ResetYVelocityOfPlayerAndGearRigidbodies();
            if(m_isGearWallJumping) _velocity += new Vector2(_gearWallJumpForceVector.x * - _physics.m_gearWallDirection, _gearWallJumpForceVector.y);
            else _velocity += new Vector2(0, _jumpForce);
            _hasJumped = true;
            _isTrigerringJump = false;
        }
    }

    private void HandlePhysicsGravityChange()
    {
        // Falling
        if (_velocity.y < 0 && _currentGravity == _initGravity && !_physics.IsGrounded()) _currentGravity = _initGravity * _fallGravityMultiplicator;
        // Normal
        else if (_velocity.y >= 0 && _currentGravity != _initGravity && _physics.IsGrounded()) _currentGravity = _initGravity;
        // Jump peak
        else if (IsOnPeakThresholdJump()) _currentGravity = _initGravity * _peakGravityMultiplicator;
        // Gear wall
        else if (_physics.IsOnContactWithGearWall()) _currentGravity = _onGearWallGearGravity;
        // Exit Gear Wall
        else if (!_physics.IsOnContactWithGearWall() && _currentGravity != _initGravity) _currentGravity = _initGravity;

        if (_physics.IsOnContactWithGear() && !_physics.IsCeiling())
        {
            _hasJumped = false;
            _isCoyoteTimerStarted = false;
        }
    }
    private void HandlePhysicsGravity()
    {
        if (_physics.IsGrounded() && _velocity.y <= 0.01f)
        {
            _velocity.y = 0;
        }
        else
        {
            _velocity.y = (_velocity.y - _currentGravity);
        }
    }

    private void ClampVelocity()
    {
        if(_physics.IsOnContactWithGear())
        {
            _velocity = new Vector2(
                Mathf.Clamp(_velocity.x, -_velocityXMax, _velocityXMax)
                , Mathf.Clamp(_velocity.y, _onGearVelocityYCap, _velocityYJumpMax)
             );
        }
        else if (_physics.IsOnContactWithGearWall() && !m_isGearWallJumping)
        {
            _velocity = new Vector2(
                Mathf.Clamp(_velocity.x, -_velocityXMax, _velocityXMax)
                , Mathf.Clamp(_velocity.y, _onGearWallVelocityYCap , _velocityYJumpMax)
             );

            if (m_inputX == 0) _velocity.y = 0;
        }
        else
        {
            _velocity = new Vector2(
                Mathf.Clamp(_velocity.x, -_velocityXMax, _velocityXMax)
                , Mathf.Clamp(_velocity.y, _velocityYFallMax, _velocityYJumpMax)
             );
        }
    }
    #endregion

    private void ResetYVelocityOfPlayerAndGearRigidbodies()
    {
        _velocity = new Vector2(_velocity.x, 0);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0);
    }

    bool IsOnPeakThresholdJump()
    {
        return (!_physics.IsGrounded() && _velocity.y > 0.01f && _hasJumped && _velocity.y < Mathf.Abs(_yVelocityPeakThreshold));
    }


}
