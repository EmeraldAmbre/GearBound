using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool _isNextGear = false;
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

    // Possession settings
    public bool m_canPossess { get; private set; }

    PlayerController _controller;
    Rigidbody2D _rb;

    void Start() {
        _controller = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        m_canPossess = false;
    }

    void Update() {
        Collider2D[] objetsDetectes1 = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _detectionLayer);
        if (objetsDetectes1.Length > 0) _isNextGear = true;
        else _isNextGear = false;

        if (_dashCooldownRemaining > 0) {
            _dashCooldownRemaining -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.H) && PlayerPrefs.HasKey("possession")) {
            if (PlayerPrefs.GetInt("magnet") == 1) m_canPossess = !m_canPossess;
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
}
