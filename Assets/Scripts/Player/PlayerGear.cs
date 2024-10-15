using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGear : MonoBehaviour
{
    [SerializeField] Transform _groundCheckLimitPoint;
    [SerializeField] PlayerCompositePhysics _playerPhysics;
    public Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 8) && (collision.GetContact(0).point.y < _groundCheckLimitPoint.position.y)) _playerPhysics.m_isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!_playerPhysics.m_isCollidingWithGround) _playerPhysics.m_isGrounded = false;
    }
}
