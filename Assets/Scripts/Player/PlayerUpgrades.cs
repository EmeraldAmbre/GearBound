using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUpgrades : MonoBehaviour {

    // Detect gears to stop or activate some upgrades
    [Header("Detection settings")]
    [SerializeField] float _detectionRay = 1.5f; // In editor, set a value big enough to detect gears (something near 1f ~ 1.2f)
    [SerializeField] LayerMask _detectionLayer; // Always set to "gear" layer in editor

    [Header("Dash settings")]
    [SerializeField] float _dashSpeed = 75f;
    [SerializeField] float _dashCooldown = 1.5f;
    [SerializeField] float _dashDuration = 0.15f;
    [SerializeField] float _gearRotationDashMultiplier = 2.5f;
    [HideInInspector] public bool m_isDashing { get; private set; } = false;
    bool _isNextGear = false; // Block the dash so the player can't enter in a fixed gear
    float _dashTimeRemaining;
    float _dashCooldownRemaining = 0f;
    float _initialSpeed;

    //[Header("Magnet settings")]
    public GameObject m_magnet { get; private set; }
    public float m_attractionForce { get; private set; } = 6;
    public bool m_isAttracted { get; private set; } = false;
    public bool m_canBeAttracted { get; private set; } = false;

    [Header("Possession settings")]
    [SerializeField] Color _intermediateColor = Color.white;
    [SerializeField] Vector3 _intermediatePos = new (1999.9f, 1999.9f, 0);
    public GameObject m_gearToControl; // there's no need to drag and drop smthg here, unless you want to customize player experience
    [SerializeField] float _xDamping = 4f; // damping (speed) following x axis of the virtual camera
    [SerializeField] float _yDamping = 4f; // damping (speed) following y axis of the virtual camera
    [SerializeField] float _zDamping = 4f; // useless still this is a 2D game
    [SerializeField] CinemachineVirtualCamera _virtualCamera; // Drag and drop here the virtual camera from package (do not forget to change the settings to "composer")
    [SerializeField] SpriteRenderer _spriteRendererFromPlayerPrefab;
    [SerializeField] SpriteRenderer _spriteRendererFromGearToControlPrefab;
    [SerializeField] bool _isPlayerFreezeWhenPossessed; // Check or uncheck this if you want that the player is freezed or not while possessing another gear
    [HideInInspector] public bool m_canControl = false;
    public bool m_isPossessed { private set; get; } = false;

    [Header("Sizing settings")]
    [SerializeField] float _growMultiplier = 2f;
    [SerializeField] float _shrinkMultiplier = 0.5f;
    Vector3 _normalSize;
    Vector3 _growSize;
    Vector3 _shrinkSize;
    int _sizeMode;

    [SerializeField] ParticleSystem _magnetismParticules;


    [SerializeField] AudioClip _sfxDash;
    [SerializeField] AudioClip _sfxMagnetActivaction;
    [SerializeField] AudioClip _sfxMagnetDeactivaction;
    [SerializeField] AudioClip _sfxReverseRotationActivaction;
    [SerializeField] AudioClip _sfxReverseRotationDeactivaction;
    [SerializeField] AudioClip _sfxPossesionActivaction;
    [SerializeField] AudioClip _sfxPossesionDeactivaction;


    Rigidbody2D _rb;
    PlayerInputAction _input;
    PlayerController _controller;
    PlayerCompositePhysics _physics;


    [Header("UI")]
    [SerializeField] GameObject _upgradeImageUiRotation;
    [SerializeField] GameObject _upgradeImageUiMagnet;
    [SerializeField] GameObject _upgradeImageUiPossesion;


    #region Input Methods

    void InitInput() {
        _input = new();
        _input.Player.Dash.performed += OnPerformDashStarted;
        _input.Player.Rotation.performed += OnPerformRotationStarted;
        _input.Player.Magnet.performed += OnPerformMagnetStarted;
        _input.Player.Size.performed += OnPerformSizeStarted;
        _input.Player.Possess.started += OnPerformPossessionStarted;
        _input.Player.GodModeDash.performed += OnPerformGodModeDash;
        _input.Player.GodModeMagnet.performed += OnPerformGodModeMagnet;
        _input.Player.GodModeRotation.performed += OnPerformGodModeRotation;
        _input.Player.GodModePossess.performed += OnPerformGodModePossession;
        _input.Player.GodModeSize.performed += OnPerformGodModeSize;
        _input.Enable();
    }

    void OnPerformDashStarted(InputAction.CallbackContext context) {
        if (_dashCooldownRemaining <= 0 && (PlayerPrefs.GetInt("dash") == 1 && PlayerPrefs.HasKey("dash"))) {
            StartDash();
        }
    }

    void OnPerformRotationStarted(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("rotation") == 1 && PlayerPrefs.HasKey("rotation")) {
            InverseRotation();
            _upgradeImageUiRotation.SetActive(_controller.m_rotationInversion);
        }
    }

    void OnPerformMagnetStarted(InputAction.CallbackContext context)
    {
        if (m_isPossessed is false)
        {
            if (PlayerPrefs.GetInt("magnet") == 1 && PlayerPrefs.HasKey("magnet"))
            {
                ActivateAttraction();
                _upgradeImageUiMagnet.SetActive(m_isAttracted);
            }
        }
    }

    void OnPerformSizeStarted(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("sizing") == 1 && PlayerPrefs.HasKey("sizing")) {
            Resizing();
        }
    }

    void OnPerformPossessionStarted(InputAction.CallbackContext context) {

        if (m_canControl && m_gearToControl != null && !m_isAttracted) {
            if (m_isPossessed is false) Possess();
            else Depossess();
            _upgradeImageUiPossesion.SetActive(m_isPossessed);
        }
    }

    void OnPerformGodModeDash(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("dash") == 0) {
            PlayerPrefs.SetInt("dash", 1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("dash") == 1) {
            PlayerPrefs.SetInt("dash", 0);
            PlayerPrefs.Save();
        }
    }

    void OnPerformGodModeMagnet(InputAction.CallbackContext context) {

        if (PlayerPrefs.GetInt("magnet") == 0)
        {
            PlayerPrefs.SetInt("magnet", 1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("magnet") == 1)
        {
            PlayerPrefs.SetInt("magnet", 0);
            PlayerPrefs.Save();
        }
        
    }

    void OnPerformGodModeRotation(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("rotation") == 0) {
            PlayerPrefs.SetInt("rotation", 1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("rotation") == 1) {
            PlayerPrefs.SetInt("rotation", 0);
            PlayerPrefs.Save();
        }
    }

    void OnPerformGodModePossession(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("possession") == 0) {
            PlayerPrefs.SetInt("possession", 1);
            PlayerPrefs.Save();
        }
        else if ( PlayerPrefs.GetInt("possession") == 1) {
            PlayerPrefs.SetInt("possession", 0);
            PlayerPrefs.Save();
        }
    }

    void OnPerformGodModeSize(InputAction.CallbackContext context) {
        if (PlayerPrefs.GetInt("sizing") == 0) {
            PlayerPrefs.SetInt("sizing", 1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("sizing") == 1) {
            PlayerPrefs.SetInt("sizing", 0);
            PlayerPrefs.Save();
        }
    }

    void OnDestroy() {
        _input.Player.Dash.performed -= OnPerformDashStarted;
        _input.Player.Rotation.performed -= OnPerformRotationStarted;
        _input.Player.Magnet.performed -= OnPerformMagnetStarted;
        _input.Player.Size.performed -= OnPerformSizeStarted;
        _input.Player.Possess.started -= OnPerformPossessionStarted;
        _input.Player.GodModeDash.performed -= OnPerformGodModeDash;
        _input.Player.GodModeMagnet.performed -= OnPerformGodModeMagnet;
        _input.Player.GodModeRotation.performed -= OnPerformGodModeRotation;
        _input.Player.GodModePossess.performed -= OnPerformGodModePossession;
        _input.Player.GodModeSize.performed -= OnPerformGodModeSize;
        _input.Player.Disable();
    }
    #endregion

    void Start() {
        _controller = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _physics = GetComponent<PlayerCompositePhysics>();

        // Sizing Initialization
        _sizeMode = 1;
        _normalSize = transform.localScale;
        _growSize = _normalSize * _growMultiplier;
        _shrinkSize = _normalSize * _shrinkMultiplier;
        InitInput();

        // Enable ui image depending current upgrade activated
        _upgradeImageUiRotation.SetActive(_controller.m_rotationInversion);
        _upgradeImageUiMagnet.SetActive(m_isAttracted);
        _upgradeImageUiPossesion.SetActive(m_isPossessed);
    }

    void Update() {

        Collider2D[] objetsDetectes1 = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes1.Length > 0) _isNextGear = true;
        else _isNextGear = false;

        if (_dashCooldownRemaining > 0) {
            _dashCooldownRemaining -= Time.deltaTime;
        }

        if (PlayerPrefs.HasKey("possession")) {
            if (PlayerPrefs.GetInt("possession") == 1) m_canControl = true;
        }


        if (m_isDashing) {
            _dashTimeRemaining -= Time.deltaTime;

            if (_dashTimeRemaining <= 0 || _physics.IsOnWall()) {
                EndDash();
            }
        }

        // ShowParticules
        if(m_isAttracted && !_magnetismParticules.isPlaying)
        {
            Debug.Log("Should play particules");
            _magnetismParticules.Play();
        }
        else if(!m_isAttracted && _magnetismParticules.isPlaying)
        {
            _magnetismParticules.Stop();
        }
        

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            m_canBeAttracted = true;
            m_magnet = other.gameObject;
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
  

        if (other.gameObject.CompareTag("Magnet"))
        {
            // m_magnet = null;
            m_canBeAttracted = false;

        }

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRay);
    }

    #region Dash Methods
    void StartDash()
    {
        m_isDashing = true;
        _dashTimeRemaining = _dashDuration;
        _dashCooldownRemaining = _dashCooldown;
        _initialSpeed = _controller.m_currentSpeed;
        _controller.SetCurrentSpeed(_dashSpeed);
        _controller.m_gearRotationDashMultiplier = _gearRotationDashMultiplier;
        AudioManager.Instance.PlaySfx(_sfxDash,2);
    }

    void EndDash() {
        _controller.SetCurrentSpeed(_initialSpeed);
        _controller.m_gearRotationDashMultiplier = 1;
        m_isDashing = false;
    }

    #endregion

    #region Rotation Methods
    void InverseRotation() {
        _controller.m_rotationInversion = !_controller.m_rotationInversion;

        // Sfx
        if(_controller.m_rotationInversion )
        {
            AudioManager.Instance.PlaySfx(_sfxReverseRotationActivaction,3);
        }
        else
        {
            AudioManager.Instance.PlaySfx(_sfxReverseRotationDeactivaction,3);
        }
    }
    #endregion

    #region Magnet Methods
    void ActivateAttraction() {
        m_isAttracted = !m_isAttracted;


        if (m_canBeAttracted == true)
        {
            _controller.ResetVelocity();
        }

        if (m_isAttracted)
        {
            AudioManager.Instance.PlaySfx(_sfxMagnetActivaction,4);
        }
        else
        {
            AudioManager.Instance.PlaySfx(_sfxMagnetDeactivaction,4);
        }
    }


    #endregion

    #region Possession Methods
    void Possess() {
        // Camera
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_XDamping = _xDamping;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_YDamping = _yDamping;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_ZDamping = _zDamping;

        // Positions
        Vector3 player_pos = transform.position;
        Vector3 gear_pos = m_gearToControl.transform.position;
        m_gearToControl.transform.position = _intermediatePos;
        transform.position = gear_pos;
        m_gearToControl.transform.position = player_pos;

        // Sprites colors
        SpriteRenderer gear_sprite = m_gearToControl.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer player_sprite = GetComponentInChildren<SpriteRenderer>();
        _intermediateColor = gear_sprite.color;
        gear_sprite.color = player_sprite.color;
        player_sprite.color = _intermediateColor;

        // Rigidbodies
        Rigidbody2D rb = m_gearToControl.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;

        m_isPossessed = true;

        AudioManager.Instance.PlaySfx(_sfxPossesionActivaction,5);
    }

    void Depossess() {
        // Camera
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_XDamping = 1;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_YDamping = 1;
        _virtualCamera.GetComponentInChildren<CinemachineTransposer>().m_ZDamping = 1;

        // Positions
        Vector3 player_pos = transform.position;
        Vector3 gear_pos = m_gearToControl.transform.position;
        m_gearToControl.transform.position = _intermediatePos;
        transform.position = gear_pos;
        m_gearToControl.transform.position = player_pos;

        // Sprites colors
        SpriteRenderer gear_sprite = m_gearToControl.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer player_sprite = GetComponentInChildren<SpriteRenderer>();
        _intermediateColor = gear_sprite.color;
        gear_sprite.color = player_sprite.color;
        player_sprite.color = _intermediateColor;

        // Rigidbodies
        Rigidbody2D rb = m_gearToControl.GetComponent<Rigidbody2D>();
        if(m_gearToControl.name.Contains("Magnet")) rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else rb.constraints = RigidbodyConstraints2D.FreezePosition;

        m_isPossessed = false;

        AudioManager.Instance.PlaySfx(_sfxPossesionDeactivaction, 5);
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
                transform.localScale = _shrinkSize;
                _sizeMode = 0;
                break;
            //case 2:
            //    transform.localScale = _growSize;
            //    _sizeMode = 0;
            //    break;
        }
    }
    #endregion
}
