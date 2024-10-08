using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompositePhysics : MonoBehaviour {

    [SerializeField] float _jumpForce = 10f;

    public bool m_isGrounded;

    Rigidbody2D _playerRigidbody;

    void Start() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 6) m_isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == 6) m_isGrounded = false;
    }

    public void Jump() {
        _playerRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
