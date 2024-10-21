using GearFactory;
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
    [SerializeField] float _velocityYOnGearMax = -20;

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
    [SerializeField] float _gearRotationSpeed = 20f;
    float _currentRotation = 0;
    [SerializeField] GameObject _body;
    Quaternion _bodyInitialRotation;

    float inputX;
    bool _isTrigerringJump = false;
    bool _hasJumped = false;
    bool _isHandlingJumpButton = false;





    void OnGearExitingOtherGear()
    {
        if (!_isTrigerringJump && !_hasJumped && !_isHandlingJumpButton)
        {
            _rigidbody.velocity = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            Debug.Log("OnGearExitingOtherGear");
        }
    }


    PlayerInputAction _input;

    void Start()
    {
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
        inputX = context.ReadValue<Vector2>().normalized.x;
    }

    private void OnPerformJumpCanceled(InputAction.CallbackContext context)
    {
        _isHandlingJumpButton = false;
    }

    private void OnPerformJumpStarted(InputAction.CallbackContext context)
    {
        if ((_physics.IsGrounded() || _physics.IsOnContactWithGear()) && !_hasJumped)
        {
            _isTrigerringJump = true;
        }
        if (!(_physics.IsGrounded() || _physics.IsOnContactWithGear()))
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
        if (_physics.IsGrounded() && _velocity.y <= 0.01f)
        {
            _hasJumped = false;
            _isCoyoteTimerStarted = false;
        }
    }
    #endregion


    void FixedUpdate()
    {
        // ORDER HAVE IMPORTANCE DON'T CHANGE THE ORDER UNLESS YOU KNOW WHAT YOU DO
        //_rigidbody.velocity = Vector2.zero;


        HandleCheckSlopePhysicsMaterialReset();


        HandlePhysicsGravityChange();
        HandlePhysicsGravity();


        HandlePhysicsXMovement();

        HandlePhysicsVelocityWenJumpTriggered();
        HandlePhysicsVelocityWhenJumpHandling();


        HandleCheckCeilingVelocityReset();

        ClampVelocity();

        //_rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);

        Debug.Log("Velocity y " + _velocity.y + "Velocity x " + _velocity.x);
        _rigidbody.velocity = _velocity;
        //transform.position = _gear.transform.position;

        UpdateGearTransformAndRotation();
    }

    private void LateUpdate()
    {

        _body.transform.rotation = _bodyInitialRotation;
    }

    bool IsOnPeakThresholdJump()
    {
        return (!_physics.IsGrounded() && _velocity.y > 0.01f && _hasJumped && _velocity.y < Mathf.Abs(_yVelocityPeakThreshold));
    }

    private void HandlePhysicsXMovement()
    {
        // On ground
        if ((_physics.IsGrounded() && _velocity.y <= 0.01f) || _physics.IsOnContactWithGear())
        {
            if (inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed, _groundDeceleration * Time.fixedDeltaTime);
            else _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed, _groundAcceleration * Time.fixedDeltaTime);
        }
        // On jump peak
        else if (IsOnPeakThresholdJump())
        {
            if (inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed * _peakXMovementMultiplicator, _peakDeceleration  * Time.fixedDeltaTime);
            else _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed * _peakXMovementMultiplicator, _peakAcceleration * Time.fixedDeltaTime);
        }
        // In air
        else
        {
            if (inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed, _airDeceleration * Time.fixedDeltaTime);
            else _velocity.x = Mathf.Lerp(_velocity.x, inputX * _currentSpeed, _airAcceleration * Time.fixedDeltaTime);
        }
    }

    #region Physics methods for FixedUpdate()
    private void HandleCheckSlopePhysicsMaterialReset()
    {
        if (_physics.IsOnSlope())
        {
            if (inputX == 0) _rigidbody.sharedMaterial = _physicMaterialFullFriction;
            else _rigidbody.sharedMaterial = _physicMaterialZeroFriction;
        }
        else _rigidbody.sharedMaterial = _physicMaterialZeroFriction;
    }



    private void UpdateGearTransformAndRotation()
    {
        if (_playerManager.m_isInteracting == false)
        {
            if (inputX == 0) _currentRotation = Mathf.Lerp(_currentRotation, inputX * _gearRotationSpeed * _playerManager.m_rotationInversion, _groundDeceleration);
            else _currentRotation = Mathf.Lerp(_currentRotation, inputX * _gearRotationSpeed * _playerManager.m_rotationInversion, _groundAcceleration);

            float rotation = inputX * _gearRotationSpeed * _playerManager.m_rotationInversion;

            //_gear.transform.Rotate(Vector3.forward, -_currentRotation * Time.deltaTime );
            //_gear.GetComponent<Rigidbody2D>().AddTorque(-rotation * Time.fixedDeltaTime);
            transform.Rotate(Vector3.forward, - _currentRotation * Time.deltaTime);
            // transform.rotation -= _currentRotation * Time.deltaTime

        }
        //_gear.GetComponent<Rigidbody2D>().MovePosition(transform.position);
        // _gear.transform.position = transform.position;

    }

    private void HandleCheckCeilingVelocityReset()
    {
        if (_physics.IsCeiling() && _velocity.y > 0.1 && !_physics.IsOnWall())
        {
            _velocity.y = 0;
            _isHandlingJumpButton = false;
        }
    }



    private void HandlePhysicsVelocityWhenJumpHandling()
    {
        if (_isHandlingJumpButton && _velocity.y > 0.01f)
        {
            _velocity += new Vector2(0, _jumpHandlingVelocity);
        }
    }

    private void HandlePhysicsVelocityWenJumpTriggered()
    {
        if (_isTrigerringJump)
        {
            ResetYVelocityOfPlayerAndGearRigidbodies();
            _velocity += new Vector2(0, _jumpForce);
            _hasJumped = true;
            _isTrigerringJump = false;
        }
    }

    private void HandlePhysicsGravityChange()
    {   
        // Falling
        if (_velocity.y < 0 && _currentGravity == _initGravity && !_physics.IsGrounded()) _currentGravity = _initGravity * _fallGravityMultiplicator;
        // Normal
        else if (_velocity.y >= 0 && _currentGravity != _initGravity) _currentGravity = _initGravity;
        // Jump peak
        if (IsOnPeakThresholdJump()) _currentGravity = _initGravity * _peakGravityMultiplicator;

        // 
        if(_physics.IsOnContactWithGear() && !_physics.IsCeiling())
        {
            Debug.Log("Is colliding with gear");
            _hasJumped = false;
            _isCoyoteTimerStarted = false;
            // _currentGravity = _initGravity / 8;
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
                , Mathf.Clamp(_velocity.y, _velocityYOnGearMax, _velocityYJumpMax)
             );
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
}
