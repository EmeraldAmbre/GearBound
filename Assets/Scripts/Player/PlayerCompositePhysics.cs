using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCompositePhysics : MonoBehaviour {

    public float _jumpForce = 7.5f;

    public bool m_isGrounded { get; set; }
    public bool m_isCollidingWithGround { get; private set; }
    public Rigidbody2D m_playerRigidbody { get; private set; }
    public CapsuleCollider2D m_playerMainCollider { get; private set; }

    [SerializeField] LayerMask _groundlayer;
    [SerializeField] Transform _groundCheckLimitPoint;


    void Start() {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_playerMainCollider = GetComponent<CapsuleCollider2D>();
    }


    void OnCollisionEnter2D(Collision2D collision) {
        // Layer 6 == Ground
        if ((collision.gameObject.layer == 6) && (collision.GetContact(0).point.y < _groundCheckLimitPoint.position.y))
        {
            m_isGrounded = true;
            m_isCollidingWithGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        m_isGrounded = false;
        m_isCollidingWithGround = false;
    }

    public void Jump() {
        m_playerRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
