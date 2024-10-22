using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour {

    #region Dash variables
    [Header("Dash")]
    [SerializeField] float _dashForce = 500f;
    [SerializeField] float _dashCooldown = 2f;

    float _dashCooldownRemaining = 0f;
    Vector2 _dashDirection;
    #endregion

    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (_dashCooldownRemaining > 0) {
            _dashCooldownRemaining -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E) && _dashCooldownRemaining <= 0 && PlayerPrefs.HasKey("dash")) {
            if (PlayerPrefs.GetInt("dash") == 1) StartDash();
        }
    }

    #region Dash Methods
    void StartDash() {
        _dashCooldownRemaining = _dashCooldown;

        _dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        rb.AddForce(_dashDirection * _dashForce, ForceMode2D.Impulse);
    }
    #endregion
}
