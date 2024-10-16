using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] float _velocityJumpMax = 20000;
    [SerializeField] float _velocityFallMax = -20000;

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
    [SerializeField] GameObject _gear;
    [SerializeField] float _gearRotationSpeed = 20f;
    float _currentRotation = 0;

    float inputX;
    bool _isTrigerringJump = false;
    bool _hasJumped = false;
    bool _isHandlingJumpButton = false;


    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentGravity = _initGravity;
        _currentSpeed = _initMoveSpeed;
    }

    void Update()
    {

        HandleJump();

        HandlingCoyoteJump();

        HandleJumpBuffering();

        HandleJumpHandling();

        HandleXInput();

        ResetNeededDataWhenOnGround();

    }


    #region Jump and movement methods called in Update()
    private void HandleXInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _physics.IsGrounded() && !_hasJumped)
        {
            _isTrigerringJump = true;
        }
    }
    private void HandleJumpBuffering()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_physics.IsGrounded())
        {
            _jumpBufferTimer = 0;
        }
        else if (_jumpBufferTimer < _jumpBufferTime) _jumpBufferTimer += Time.deltaTime;
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
        if (Input.GetKeyDown(KeyCode.Space) && _jumpCoyoteTimer < _jumpCoyoteTime)
        {
            _jumpCoyoteTimer = _jumpCoyoteTime;
            ResetYVelocityOfPlayerAndGearRigidbodies();
            _isTrigerringJump = true;
        }
    }
    private void HandleJumpHandling()
    {
        if (Input.GetKey(KeyCode.Space) && !_physics.IsGrounded() && _velocity.y >= 0)
        {
            _isHandlingJumpButton = true;
        }
        else _isHandlingJumpButton = false;
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
        if (IsOnPeakThresholdJump()) Debug.Log("On jump peak");
        // ORDER HAVE IMPORTANCE DON'T CHANGE THE ORDER UNLESS YOU KNOW WHAT YOU DO
        _rigidbody.velocity = Vector2.zero;

        HandlePhysicsXMovement();

        //[SerializeField] float _yVelocityPeakThreshold = 10f;
        //[SerializeField] float _peakGravityMultiplicator = 0.8f;
        //[SerializeField] float _peakSpeedMultiplicator = 1.2f;
        //[SerializeField] float _peakAcceleration = 1.2f;
        //[SerializeField] float _peakDeceleration = 1.2f;


        HandlePhysicsGravityChange();
        HandlePhysicsGravity();

        HandleCheckSlopePhysicsMaterialReset();


        HandlePhysicsVelocityWenJumpTriggered();

        HandlePhysicsVelocityWhenJumpHandling();

        UpdateGearTransformAndRotation();

        HandleCheckCeilingVelocityReset();


        ClampVelocity();

        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    bool IsOnPeakThresholdJump()
    {
        return (!_physics.IsGrounded() && _velocity.y > 0.01f && _hasJumped && _velocity.y < Mathf.Abs(_yVelocityPeakThreshold));
    }

    private void HandlePhysicsXMovement()
    {
        // On ground
        if (_physics.IsGrounded() && _velocity.y <= 0.01f)
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

    private void UpdateGearTransformAndRotation()
    {
        if (_playerManager.m_isInteracting == false) RotateGear();
        _gear.transform.position = transform.position;
    }

    private void HandleCheckCeilingVelocityReset()
    {
        if (_physics.IsCeiling() && _velocity.y > 0.1 && !_physics.IsOnWall())
        {
            _velocity.y = 0;
        }
    }



    private void HandlePhysicsVelocityWhenJumpHandling()
    {
        if (_isHandlingJumpButton && _velocity.y > 0)
        {
            _velocity += new Vector2(0, _jumpHandlingVelocity);
            //_rigidbody.AddForce(new Vector2(0, _jumpHandlingVelocity), ForceMode2D.Force);
        }
    }

    private void HandlePhysicsVelocityWenJumpTriggered()
    {
        if (_isTrigerringJump)
        {
            ResetYVelocityOfPlayerAndGearRigidbodies();
            //_rigidbody.AddForce(new Vector2(0 ,_physics._jumpForce), ForceMode2D.Impulse);
            _velocity += new Vector2(0, _jumpForce);
            _hasJumped = true;
            _isTrigerringJump = false;
            // Debug.Log("Jump with starting y velocity : " + _rigidbody.velocity.y + "  At time : " + Time.time);
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
    }

    private void ClampVelocity()
    {
        _velocity = new Vector2(
            Mathf.Clamp(_velocity.x, -_velocityXMax, _velocityXMax)
            , Mathf.Clamp(_velocity.y, _velocityFallMax, _velocityJumpMax)
         );
    }
    #endregion

    private void ResetYVelocityOfPlayerAndGearRigidbodies()
    {
        _velocity = new Vector2(_velocity.x, 0);
        _gear.GetComponent<Rigidbody2D>().velocity = new Vector3(_gear.GetComponent<Rigidbody2D>().velocity.x, 0);
    }

    void RotateGear() {
        if (inputX == 0) _currentRotation = Mathf.Lerp(_currentRotation, inputX * _gearRotationSpeed * _playerManager.m_rotationInversion, _groundDeceleration * Time.fixedDeltaTime);
        else _currentRotation = Mathf.Lerp(_currentRotation, inputX * _gearRotationSpeed * _playerManager.m_rotationInversion, _groundAcceleration * Time.fixedDeltaTime);

        float rotation = inputX * _gearRotationSpeed * _playerManager.m_rotationInversion;
        //_gear.transform.Rotate(Vector3.forward, -rotation);
        _gear.GetComponent<Rigidbody2D>().rotation -= _currentRotation * Time.fixedDeltaTime;
    }




}
