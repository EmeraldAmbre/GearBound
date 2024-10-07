using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _rotationSpeed = 20f;
    [SerializeField] float _gravityScale = 5f;
    [SerializeField] float _jumpHeight = 5f;
    [SerializeField] float _velocity;

    [SerializeField] PlayerPhysics _physics;

    private void FixedUpdate() {

        Jump();
        Move();
        Rotate();

    }

    void Move() {

        float inputX = Input.GetAxis("Horizontal");
        Vector3 deplacement = new Vector3(inputX * _moveSpeed * Time.deltaTime, 0, 0);
        transform.Translate(deplacement, Space.World);

    }

    void Rotate() {

        float inputRotation = Input.GetAxis("Horizontal");
        float rotation = inputRotation * _rotationSpeed;
        transform.Rotate(Vector3.forward, -rotation);

    }

    void Jump() {

        _velocity += Physics2D.gravity.y * _gravityScale * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _velocity = Mathf.Sqrt(_jumpHeight * -2 * (Physics2D.gravity.y * _gravityScale));
        }

        if (_velocity >= 0) { transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime); }

        else {
            if (!_physics.m_groundCheck) { transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime); }
        }

    }


}
