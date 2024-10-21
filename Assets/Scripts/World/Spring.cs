using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    [SerializeField] float _force = 2f;

    Animator _animator;

    public bool m_isTighten = true;
    void Start() {
        _animator = GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D collision) {

        GameObject player = collision.gameObject;

        if (player.CompareTag("Player") && m_isTighten) {

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * _force, ForceMode2D.Force);
            _animator.SetBool("Extend", true);
            m_isTighten = false;
        
        }
    }

    public void Tighten() {

        if (!m_isTighten) {

            _animator.SetBool("Extend", false);
            _animator.SetBool("Tighten", true);
            m_isTighten = true;

        }
    }
}
