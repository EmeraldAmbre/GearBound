using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour {

    // Detect gears to stop or activate some upgrades
    [Header("Detection settings")]
    [SerializeField] float _detectionRay = 1.5f; // In editor, set a value big enough to detect gears (something near 1f ~ 1.2f)
    [SerializeField] LayerMask _detectionLayer; // Always set to "gear" layer in editor

    [Header("Dash settings")]
    [SerializeField] float _dashSpeed = 75f;
    [SerializeField] float _dashCooldown = 1.5f;
    [SerializeField] float _dashDuration = 0.15f;
    bool _isDashing = false;
    bool _isNextGear = false; // Block the dash so the player can't enter in a fixed gear
    float _dashTimeRemaining;
    float _dashCooldownRemaining = 0f;
    float _initialSpeed;

    [Header("Magnet settings")]
    [SerializeField] float _attractionForce = 5f;
    [SerializeField] float _stopDistance = 1f;
    GameObject _magnet;
    bool _isFreezed = false;
    bool _isAttracted = false;
    bool _canBeAttracted = false;

    [Header("Possession settings")]
    [SerializeField] Vector3 _intermediatePos = new (1999.9f, 1999.9f, 0);
    [SerializeField] GameObject _gearToControl; // there's no need to drag and drop smthg here, unless you want to customize player experience
    [SerializeField] float _xDamping = 4f; // damping (speed) following x axis of the virtual camera
    [SerializeField] float _yDamping = 4f; // damping (speed) following y axis of the virtual camera
    [SerializeField] float _zDamping = 4f; // useless still this is a 2D game
    [SerializeField] CinemachineVirtualCamera _virtualCamera; // Drag and drop here the virtual camera from package (do not forget to change the settings to "composer")
    [SerializeField] SpriteRenderer _spriteRendererFromPlayerPrefab;
    [SerializeField] SpriteRenderer _spriteRendererFromGearToControlPrefab;
    [SerializeField] bool _isPlayerFreezeWhenPossessed; // Check or uncheck this if you want that the player is freezed or not while possessing another gear
    bool _canControl = false;
    bool _isPossessed = false;

    [Header("Sizing settings")]
    [SerializeField] float _growMultiplier = 2f;
    [SerializeField] float _shrinkMultiplier = 0.5f;
    Vector3 _normalSize;
    Vector3 _growSize;
    Vector3 _shrinkSize;
    int _sizeMode;

    // Possession settings
    

    PlayerController _controller;
    Rigidbody2D _rb;

    void Start() {
        _controller = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();

        // Sizing Initialization
        _sizeMode = 1;
        _normalSize = transform.localScale;
        _growSize = _normalSize * _growMultiplier;
        _shrinkSize = _normalSize * _shrinkMultiplier;
    }

    void Update() {
        Collider2D[] objetsDetectes1 = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes1.Length > 0) _isNextGear = true;
        else _isNextGear = false;

        if (_dashCooldownRemaining > 0) {
            _dashCooldownRemaining -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.I) && PlayerPrefs.HasKey("sizing")) {
            if (PlayerPrefs.GetInt("sizing") == 1) Resizing();
        }

        if (PlayerPrefs.HasKey("possession")) {
            if (PlayerPrefs.GetInt("possession") == 1) _canControl = true;
        }

        if (Input.GetKeyDown(KeyCode.H) && _canControl && _gearToControl != null) {
            if (_isPossessed == false) Possess();
            else Depossess();
        }

        if (Input.GetKeyDown(KeyCode.G) && PlayerPrefs.HasKey("magnet")) {
            if (PlayerPrefs.GetInt("magnet") == 1) ActivateAttraction();
        }

        if (Input.GetKeyDown(KeyCode.F) && PlayerPrefs.HasKey("rotation")) {
            if (PlayerPrefs.GetInt("rotation") == 1) InverseRotation();
        }

        if (Input.GetKeyDown(KeyCode.E) && _dashCooldownRemaining <= 0 && PlayerPrefs.HasKey("dash")) {
            if (PlayerPrefs.GetInt("dash") == 1) StartDash();
        }

        if (_isDashing) {
            _dashTimeRemaining -= Time.deltaTime;

            if (_dashTimeRemaining <= 0 || _isNextGear) {
                EndDash();
            }
        }

        if (_canBeAttracted == false) _isAttracted = false;

        if (_isAttracted == true && _isFreezed == false) {
            AttractPlayer(_magnet);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet") && _canBeAttracted) {
            _magnet = other.gameObject;
            _isAttracted = true;
        }

        if (other.gameObject.CompareTag("ControllableGear") && _canControl) {
            _gearToControl = other.gameObject;
        }

        if (other.gameObject.CompareTag("ControllableGear") && _isPossessed) {
            Rigidbody2D rb = _gearToControl.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ControllableGear") && _isPossessed) {
            Rigidbody2D rb = _gearToControl.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }

    #region Dash Methods
    void StartDash() {
        _isDashing = true;
        _dashTimeRemaining = _dashDuration;
        _dashCooldownRemaining = _dashCooldown;
        _initialSpeed = _controller.m_currentSpeed;
        _controller.SetCurrentSpeed(_dashSpeed);
    }

    void EndDash() {
        _controller.SetCurrentSpeed(_initialSpeed);
        _isDashing = false;
    }
    #endregion

    #region Rotation Methods
    void InverseRotation() {
        _controller.m_rotationInversion = !_controller.m_rotationInversion;
    }
    #endregion

    #region Magnet Methods
    void ActivateAttraction() {
        _canBeAttracted = !_canBeAttracted;

        if (_canBeAttracted == false) {
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _isFreezed = false;
        }
    }

    void AttractPlayer(GameObject magnet) {

        Vector3 direction = (magnet.transform.position - transform.position).normalized;

        if (_isFreezed == false) {
            
            transform.Translate(direction * _attractionForce * Time.deltaTime, Space.World);
            
            float distance = Vector3.Distance(transform.position, magnet.transform.position);

            if (distance < _stopDistance) {
                transform.position = magnet.transform.position - direction * _stopDistance;
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _isFreezed = true;
            }
        }
    }
    #endregion

    #region Possession Methods
    void Possess() {
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_XDamping = _xDamping;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_YDamping = _yDamping;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_ZDamping = _zDamping;
        Vector3 player_pos = transform.position;
        Vector3 gear_pos = _gearToControl.transform.position;
        _gearToControl.transform.position = _intermediatePos;
        transform.position = gear_pos;
        _gearToControl.transform.position = player_pos;
        SpriteRenderer gearSprite = _gearToControl.GetComponent<SpriteRenderer>();
        gearSprite.sprite = _spriteRendererFromPlayerPrefab.sprite;
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        playerSprite.sprite = _spriteRendererFromGearToControlPrefab.sprite;
        Rigidbody2D rb = _gearToControl.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        _isPossessed = true;
    }

    void Depossess() {
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_XDamping = 1;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_YDamping = 1;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_ZDamping = 1;
        Vector3 player_pos = transform.position;
        Vector3 gear_pos = _gearToControl.transform.position;
        _gearToControl.transform.position = _intermediatePos;
        transform.position = gear_pos;
        _gearToControl.transform.position = player_pos;
        SpriteRenderer gearSprite = _gearToControl.GetComponent<SpriteRenderer>();
        gearSprite.sprite = _spriteRendererFromGearToControlPrefab.sprite; 
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        playerSprite.sprite = _spriteRendererFromPlayerPrefab.sprite;
        Rigidbody2D rb = _gearToControl.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _isPossessed = false;
    }
    #endregion

    #region Sizing Methods
    private void Resizing() {
        switch (_sizeMode) {
            case 0:
                transform.localScale = _normalSize;
                _sizeMode += 1;
                break;
            case 1:
                transform.localScale = _growSize;
                _sizeMode += 1;
                break;
            case 2:
                transform.localScale = _shrinkSize;
                _sizeMode = 0;
                break;
        }
    }
    #endregion
}
