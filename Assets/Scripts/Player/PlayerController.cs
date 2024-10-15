using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Movement
    bool _isSpinningFixed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;


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
    [SerializeField] GameObject _gear;
    PlayerCompositePhysics _physics;
    PlayerScriptedPhysics _scriptedPhysics;
    PlayerManager _playerManager;
    Rigidbody2D _rigidbody;

    float inputX;
    bool _isTrigerringJump = false;
    bool _hasJumped = false;
    bool _isHandlingJumpButton = false;
    bool _wasGroundedOnLastFrame = false;

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
        //if (Input.GetKeyDown(KeyCode.E) && _isSpinningFixed == false && _physics.m_isGrounded)
        //{
        //    _isSpinningFixed = true;
        //    _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        //}
        //else if (Input.GetKeyDown(KeyCode.E) && _isSpinningFixed == true)
        //{
        //    _isSpinningFixed = false;
        //    _rigidbody.constraints = RigidbodyConstraints2D.None;
        //}
        //if (Input.GetKeyDown(KeyCode.F)) _playerManager.RotationInversion();

        HandleJump();

        HandlingCoyoteJump();

        HandleJumpBuffering();

        HandleJumpHandling();

        HandleXInput();

        if (_physics.IsGrounded() && _rigidbody.velocity.y <= 0.01f)
        {
            _hasJumped = false;
            ResetYVelocityOfPlayerAndGearRigidbodies();
        }

    }

    #region Jump and movement methods called in Update()
    private void HandleXInput()
    {
        if (Input.GetKey(KeyCode.A)) inputX = -1;
        else if (Input.GetKey(KeyCode.D)) inputX = 1;
        else inputX = 0;
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
        if (!_hasJumped && !_physics.IsGrounded() && _rigidbody.velocity.y < 0 && _jumpCoyoteTimer != 0)
        {
            _jumpCoyoteTimer = 0;
            Debug.Log("Coyote timer triggered");
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
        if (Input.GetKey(KeyCode.Space) && !_physics.IsGrounded() && _rigidbody.velocity.y >= 0)
        {
            _isHandlingJumpButton = true;
        }
        else _isHandlingJumpButton = false;
    } 
    #endregion


    void FixedUpdate()
    {
        if (_playerManager.m_isInteracting == false) RotateGear();

        Vector2 newVelocity = new Vector3(inputX * _moveSpeed, _rigidbody.velocity.y, 0);

        HandlePhysicsGravityChangeOnFall();

        HandlePhysicsVelocityWenJumpTriggered();

        HandlePhysicsVelocityWhenJumpHandling();

        _rigidbody.velocity += newVelocity;

        //_rigidbody.velocity = new Vector2(
        //    Mathf.Clamp(_rigidbody.velocity.x, -_velocityXMax, _velocityXMax)
        //    ,Mathf.Clamp(_rigidbody.velocity.y, _velocityFallMax, _velocityJumpMax)
        //);

        _gear.transform.position = transform.position;
    }

    #region Physics methods for FixedUpdate()
    private void HandlePhysicsVelocityWhenJumpHandling()
    {
        if (_isHandlingJumpButton && _rigidbody.velocity.y >= 0)
        {
            _rigidbody.velocity += new Vector2(0, _jumpHandlingVelocity);
            //_rigidbody.AddForce(new Vector2(0, _jumpHandlingVelocity), ForceMode2D.Force);
        }
    }

    private void HandlePhysicsVelocityWenJumpTriggered()
    {
        if (_isTrigerringJump)
        {
            ResetYVelocityOfPlayerAndGearRigidbodies();
            //_rigidbody.AddForce(new Vector2(0 ,_physics._jumpForce), ForceMode2D.Impulse);
            _rigidbody.velocity += new Vector2(0, _physics._jumpForce);
            _hasJumped = true;
            _isTrigerringJump = false;
            //Debug.Log("Jump with starting y velocity : " + _rigidbody.velocity.y + "  At time : " + Time.time);
        }
    }

    private void HandlePhysicsGravityChangeOnFall()
    {
        if (_rigidbody.velocity.y < 0 && _rigidbody.gravityScale == _initGravityScale && !_physics.IsGrounded()) _rigidbody.gravityScale *= _fallGravityMultiplicator;
        else if (_rigidbody.velocity.y >= 0 && _rigidbody.gravityScale != _initGravityScale) _rigidbody.gravityScale = _initGravityScale;
    } 
    #endregion

    private void ResetYVelocityOfPlayerAndGearRigidbodies()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, 0);
        _gear.GetComponent<Rigidbody2D>().velocity = new Vector3(_gear.GetComponent<Rigidbody2D>().velocity.x, 0, 0);
    }

    void RotateGear() {
        float rotation = inputX * _rotationSpeed * _playerManager.m_rotationInversion;
        _gear.transform.Rotate(Vector3.forward, -rotation);
    }


  

}
