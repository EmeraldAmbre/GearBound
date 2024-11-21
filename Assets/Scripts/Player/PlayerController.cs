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
    [SerializeField] float _groundAcceleration = 7f;
    [SerializeField] float _groundDeceleration = 17f;
    [SerializeField] float _airAcceleration = 7f;
    [SerializeField] float _airDeceleration = 17f;
    [SerializeField] float _velocityXMax = 20000;
    [SerializeField] float _velocityYJumpMax = 20000;
    [SerializeField] float _velocityYFallMax = -150;
    public float m_currentSpeed { get; private set; }
    public bool m_rotationInversion { get; set; }

    [Header("Jump values")]
    [SerializeField] float _jumpForce = 7.5f;
    [SerializeField] float _jumpHandlingVelocity = 5;
    [SerializeField] float _jumpBufferTime = 0.13f;
    [SerializeField] float _jumpCoyoteTime = 0.13f;
    float _jumpBufferTimer = 5;
    float _jumpCoyoteTimer = 5;
    bool _isCoyoteTimerStarted = false;

    [Header("Air peak values")]
    [SerializeField] float _yVelocityPeakThreshold = 10f;
    [SerializeField] float _peakGravityMultiplicator = 0.8f;
    [SerializeField] float _peakXMovementMultiplicator = 1.2f;
    [SerializeField] float _peakAcceleration = 1.2f;
    [SerializeField] float _peakDeceleration = 1.2f;


    [Header("Physics")]
    [SerializeField] float _initGravity = 9.81f;
    [SerializeField] float _fallGravityMultiplicator = 2;
    [SerializeField] PhysicsMaterial2D _physicMaterialFullFriction;
    [SerializeField] PhysicsMaterial2D _physicMaterialZeroFriction;
    float _currentGravity;
    Rigidbody2D _rigidbody;
    PlayerManager _playerManager;
    PlayerCompositePhysics _physics;
    Vector2 _velocity = Vector2.zero;

    // Physics
    [Header("Gear")]
    [SerializeField] float _onGearSpeedMultiplicator = 0.7f;
    [SerializeField] float _onGearVelocityYCap = -3;
    [SerializeField] float _onGearWallSpeedMultiplicator = 5f;
    [SerializeField] float _onGearWallVelocityYCap = 0;
    [SerializeField] float _gearGroundRotationSpeed = 20f;
    [SerializeField] float _gearAirRotationSpeed = 20f;
    [SerializeField] float _onGearWallGearRotationSpeed = 300f;
    [SerializeField] float _gearRotationAcceleration = 7f;
    [SerializeField] float _gearRotationDeceleration = 17f;
    [SerializeField] float _onGearWallGearGravity = 0;
    [SerializeField] float _airGearWallJumpAcceleration = 7f;
    [SerializeField] float _airGearWallJumpDeceleration = 17f;
    [SerializeField] Vector2 _gearWallJumpForceVector = Vector2.zero;
    [SerializeField] GameObject _body;
    public bool m_isGearWallJumping = false;
    public float m_currentGearRotation { get; private set; } = 0;
    public float m_gearRotationDashMultiplier { get; set; } = 1;

    int _lastDirection = 0;

    Quaternion _bodyInitialRotation;

    public float m_inputX { get; private set; } = 0;
    bool _isTrigerringJump = false;
    bool _hasJumped = false;
    bool _isHandlingJumpButton = false;

    PlayerInputAction _input;
    PlayerUpgrades _playerUpgrade;

    bool _enableGodMode;


    [SerializeField] ParticleSystem _landingParticules;
    [SerializeField] ParticleSystem _walkParticules;
    [SerializeField] float _walkParticulesPeriod = 0.1f;
    float _walkParticulesCounter = 0;

    [SerializeField] List<AudioClip> _listSfxJump;
    [SerializeField] List<AudioClip> _listSfxFootStep;
    [SerializeField] float _footStepDelay = 0.2f;
    float _footStepTimer = 0;

    void Start() {
        _physics = GetComponent<PlayerCompositePhysics>();
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentGravity = _initGravity;
        InitInput();
        _bodyInitialRotation = _body.transform.rotation;
        m_currentSpeed = _initMoveSpeed;
        _playerUpgrade = GetComponent<PlayerUpgrades>();
    }

    #region Input Methods

    void InitInput() {
        _input = new();
        _input.Player.Jump.started += OnPerformJumpStarted;
        _input.Player.Jump.canceled += OnPerformJumpCanceled;
        _input.Player.Movement.performed += OnPerformXAxis;
        _input.Player.GodModeMove.performed += OnPerformGodModeMove;
        _input.Enable();
    }

    void OnPerformXAxis(InputAction.CallbackContext context) {
        if(_playerUpgrade.m_isDashing is false)
        {
            if (context.ReadValue<Vector2>().normalized.x > 0) m_inputX = 1;
            else if (context.ReadValue<Vector2>().normalized.x < 0) m_inputX = -1;
            else m_inputX = 0;
        }
    }

    void OnPerformJumpCanceled(InputAction.CallbackContext context) {
        _isHandlingJumpButton = false;
    }

    void OnPerformJumpStarted(InputAction.CallbackContext context) {

        if ((_physics.IsGrounded() || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall()) && !_hasJumped) {
            _isTrigerringJump = true;
            if (_physics.IsOnContactWithGearWall() && !_physics.IsGrounded()) m_isGearWallJumping = true;
        }

        if (!(_physics.IsGrounded() || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall())) {
            _jumpBufferTimer = 0;
        }

        if (_jumpCoyoteTimer < _jumpCoyoteTime) {
            _jumpCoyoteTimer = _jumpCoyoteTime;
            ResetYVelocityOfPlayerAndGearRigidbodies();
            _isTrigerringJump = true;
        }

        _isHandlingJumpButton = true;

        if(_isTrigerringJump)
        {
            AudioManager.Instance.PlayRandomSfx(_listSfxJump, 0);
        }

    }

    void OnPerformGodModeMove(InputAction.CallbackContext context) {
        _enableGodMode = !_enableGodMode;
        if (_enableGodMode) {
            _rigidbody.isKinematic = true;
        }
        else {
            _rigidbody.isKinematic = false;
        }
    }

    void OnDestroy() {
        _input.Player.Jump.started -= OnPerformJumpStarted;
        _input.Player.Jump.canceled -= OnPerformJumpCanceled;
        _input.Player.Movement.performed -= OnPerformXAxis;
        _input.Player.GodModeMove.performed -= OnPerformGodModeMove;
        _input.Player.Disable();
    } 
    #endregion

    void Update() {
        HandlingCoyoteJump();

        HandleJumpBuffering();

        ResetNeededDataWhenOnGround();

        // Particules
        _walkParticulesCounter += Time.deltaTime;
        if (Mathf.Abs(m_inputX) > 0.1 && _physics.IsGrounded() && !_physics.IsOnContactWithGear() && !_physics.IsOnContactWithGearWall() && _walkParticulesCounter > _walkParticulesPeriod)
        {
            _walkParticules.Play();
            _walkParticulesCounter = 0;
        }

        if(!_physics.m_wasGroundedOnLastFrame && _physics.IsGrounded() && !_physics.IsOnContactWithGear() && !_physics.IsOnContactWithGearWall())
        {
            _landingParticules.Play();
        }

        // Sfx footstep
        if(Mathf.Abs(m_inputX) > 0.1 && _physics.IsGrounded() )
        {
            if(_footStepTimer == 0)
            {
                AudioManager.Instance.PlayRandomSfx(_listSfxFootStep, 0);
            }
            if (_footStepTimer < _footStepDelay)
            {
                _footStepTimer += Time.deltaTime;
            }
            else
            {
                _footStepTimer = 0;
            }
        }
        else if (_footStepTimer != 0) _footStepTimer = 0;
    }

    #region Jump and movement methods called in Update()

    private void HandleJumpBuffering() {

        if (_jumpBufferTimer < _jumpBufferTime) _jumpBufferTimer += Time.deltaTime;

        if (_physics.IsGrounded() && _jumpBufferTimer < _jumpBufferTime) {
            _isTrigerringJump = true;
            _jumpBufferTimer = _jumpBufferTime;
        }
    }

    private void HandlingCoyoteJump() {

        if (!_hasJumped && !_physics.IsGrounded() && _velocity.y < 0 && !_isCoyoteTimerStarted) {
            _jumpCoyoteTimer = 0;
            _isCoyoteTimerStarted = true;
        }

        else if (_jumpCoyoteTimer < _jumpCoyoteTime) _jumpCoyoteTimer += Time.deltaTime;
   
    }

    private void ResetNeededDataWhenOnGround() {

        if ((_physics.IsGrounded() && _velocity.y <= 0.01f) || _physics.IsOnContactWithGear() || _physics.IsOnContactWithGearWall()) {

            _hasJumped = false;
            _isCoyoteTimerStarted = false;

            if (!_physics.IsOnContactWithGearWall()) m_isGearWallJumping = false;
        }
    }
    #endregion

    void HandleMagnetAttraction()
    {
        //Debug.Log("Player can be attracted : " + _playerUpgrade.m_canBeAttracted);
        //Debug.Log("Player is attracted : " + _playerUpgrade.m_isAttracted);
        //Debug.Log("_velocity : " + _velocity);



        if (_playerUpgrade.m_canBeAttracted is true && _playerUpgrade.m_isAttracted is true)
        {
            Vector2 direction = (_playerUpgrade.m_magnet.transform.position - transform.position).normalized ;
            _velocity = _playerUpgrade.m_attractionForce * direction;
            
        }
    }

    void FixedUpdate() {

        if (_enableGodMode is false) 
        {
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
            HandleMagnetAttraction();

            ClampVelocity();




        // _rigidbody.MovePosition((Vector2) transform.position + _velocity * Time.fixedDeltaTime);
        _rigidbody.velocity = _velocity;

            // _rigidbody.MovePosition((Vector2) transform.position + _velocity * Time.fixedDeltaTime);
            _rigidbody.velocity = _velocity;

            UpdateGearRotation();
        }

        else {
            Vector3 moveDirection = Vector3.zero;
            transform.rotation = Quaternion.identity;

            if (Input.GetKey(KeyCode.UpArrow) )
            {
                moveDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.DownArrow) )
            {
                moveDirection += Vector3.down;
            }
            if (Input.GetKey(KeyCode.LeftArrow) )
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow) )
            {
                moveDirection += Vector3.right;
            }

            transform.Translate(moveDirection.normalized * m_currentSpeed * 0.05f);
        }
    }

    #region Physics methods for FixedUpdate()
    private void HandlePhysicsXMovement() {

        if (m_inputX != 0) _lastDirection = (int)m_inputX;

        // On dash
        if (_playerUpgrade.m_isDashing)
        {
            _velocity.x = _lastDirection * m_currentSpeed * _initMoveSpeed;
        }
        else if (_physics.IsOnSlope() && _physics.IsGrounded() && _velocity.y <= 0.01f)
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _groundDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _groundAcceleration);
        }

        // On ground
        else if ((_physics.IsGrounded() && _velocity.y <= 0.01f))
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _groundDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _groundAcceleration);
        }

        // On jump peak
        else if (IsOnPeakThresholdJump())
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _peakXMovementMultiplicator, _peakDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _peakXMovementMultiplicator, _peakAcceleration);
        }

        // On gear
        else if (_physics.IsOnContactWithGear())
        {
            // _velocity.x = 0;
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _onGearSpeedMultiplicator, _groundDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _onGearSpeedMultiplicator, _groundAcceleration);
        }

        // On gear wall
        else if (_physics.IsOnContactWithGearWall() && !m_isGearWallJumping)
        {
            if (m_inputX == 0) { _velocity.x = 0; }
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _onGearWallSpeedMultiplicator, _groundAcceleration);
        }

        // In air when gear wall jumping
        else if (m_isGearWallJumping)
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _onGearWallSpeedMultiplicator, _airGearWallJumpDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed * _onGearWallSpeedMultiplicator, _airGearWallJumpAcceleration);
        }

        // In air
        else
        {
            if (m_inputX == 0) _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _airDeceleration);
            else _velocity.x = Mathf.Lerp(_velocity.x, m_inputX * m_currentSpeed, _airAcceleration);
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
        
        float targetRotationSpeed = 0;
        
        // Check which gear rotation to apply
        if (_physics.IsOnContactWithGearWall()) targetRotationSpeed = _onGearWallGearRotationSpeed;
        else if (_physics.IsOnContactWithGear()) targetRotationSpeed = _gearAirRotationSpeed;
        else if (_physics.IsGrounded()) targetRotationSpeed = _gearGroundRotationSpeed;
        else targetRotationSpeed = _gearAirRotationSpeed;
        
        // Set the gear rotation lerp
        if (m_inputX == 0) m_currentGearRotation = Mathf.Lerp(m_currentGearRotation, m_inputX * targetRotationSpeed, _gearRotationDeceleration);
        else m_currentGearRotation = Mathf.Lerp(m_currentGearRotation, m_inputX * targetRotationSpeed, _gearRotationAcceleration);

        // Apply the gear rotation
        if (!m_rotationInversion) transform.Rotate(Vector3.forward, -m_currentGearRotation * m_gearRotationDashMultiplier);
        else transform.Rotate(Vector3.forward, m_currentGearRotation * m_gearRotationDashMultiplier);

        _body.transform.rotation = _bodyInitialRotation;
    }

    private void HandleCheckCeilingVelocityReset()
    {
        if (_physics.IsCeiling() && _velocity.y > 0.1 && (! _physics.IsGrounded() && ! _physics.IsOnWallAndWallGears() ) )
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
        // Commented to avoid strange player not jumping behavior
        if (_physics.IsGrounded() && _velocity.y <= 0.01f || (_playerUpgrade.m_canBeAttracted && _playerUpgrade.m_isAttracted))
        {
            _velocity.y = 0;
        }
        else
        {
            _velocity.y = (_velocity.y - _currentGravity);
        }
        //_velocity.y = (_velocity.y - _currentGravity);
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

        // Disable gravity on dash
        if(_playerUpgrade.m_isDashing)
        {
            _velocity = new Vector2(
                Mathf.Clamp(_velocity.x, -_velocityXMax, _velocityXMax)
                , Mathf.Clamp(_velocity.y, 0, 0)
            );
        }
    }
    #endregion

    private void ResetYVelocityOfPlayerAndGearRigidbodies()
    {
        _velocity = new Vector2(_velocity.x, 0);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0);
    }

    bool IsOnPeakThresholdJump() {
        return (!_physics.IsGrounded() && _velocity.y > 0.01f && _hasJumped && _velocity.y < Mathf.Abs(_yVelocityPeakThreshold));
    }

    public void SetCurrentSpeed(float speed) {
        m_currentSpeed = speed;
    }

    public void ResetVelocity()
    {
        _velocity = Vector2.zero;
        _rigidbody.velocity = _velocity;
        Debug.Log("RESET VELOCITY");

    }
}
