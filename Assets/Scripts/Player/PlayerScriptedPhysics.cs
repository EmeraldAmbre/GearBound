using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptedPhysics : MonoBehaviour {

    [SerializeField] bool _isGrounded;
    [SerializeField] float _gravityScale = 5f;
    [SerializeField] float _jumpHeight = 5f;
    [SerializeField] float _jumpDuration = 1f;
    [SerializeField] float _velocity;
    [SerializeField] float _detectionRay = 1f;
    [SerializeField] LayerMask _layerGround;

    float _jumpTimer;
    bool _isJumping;
    Vector3 _initialPos;

    void Update() {

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, _detectionRay, _layerGround);

        if (detectedObjects.Length > 0) _isGrounded = true;

        else _isGrounded = false;

    }

    void FixedUpdate() {

        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping) {
            BeginJump();
        }

        else if (_isJumping) {
            ScriptedJump();
        }

        else {
            GravityFalls();
        }
    }

    void GravityFalls() {

        _velocity += Physics2D.gravity.y * _gravityScale * Time.deltaTime;

        if (_isGrounded && _velocity < 0) {
            _velocity = 0;
        }

        else {
            transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime);
        }
    }

    void BeginJump() {
        _isJumping = true;
        _jumpTimer = 0f;
        _initialPos = transform.position;
    }

    void ScriptedJump() {
        _jumpTimer += Time.deltaTime;
        float progression = _jumpTimer / _jumpDuration;

        if (progression >= 1f) {
            _isJumping = false;
            transform.position = new Vector3(transform.position.x, _initialPos.y, transform.position.z);
            return;
        }

        float newHeight = _initialPos.y + 4 * _jumpHeight * (progression * (1 - progression));

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _detectionRay);
    //}
}
