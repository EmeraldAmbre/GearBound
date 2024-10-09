using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompositePhysics : MonoBehaviour {

    [SerializeField] float _jumpForce = 10f;

    public bool m_isGrounded { get; private set; }
    public Rigidbody2D m_playerRigidbody { get; private set; }
    public CircleCollider2D m_playerMainCollider { get; private set; }

    void Start() {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_playerMainCollider = GetComponent<CircleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 6) m_isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == 6) m_isGrounded = false;
    }

    public void Jump() {
        m_playerRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
