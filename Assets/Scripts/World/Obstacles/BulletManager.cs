using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public bool m_isGoingRight;
    public float m_lifeTime = 10f;

    float _timer;
    [SerializeField] float _speed = 2f;
    bool _rotated;

    GameObject _player;
    PlayerManager _manager;

    void Awake() {
        _timer = m_lifeTime;
        _rotated = false;
        _player = GameObject.FindWithTag("Player");
        _manager = _player.GetComponent<PlayerManager>();
    }

    void Update() {

        _timer -= Time.deltaTime;

        if (_rotated == false) {
            if (m_isGoingRight == false) transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            _rotated = true;
        }

        if (_timer > 0) {
            if (m_isGoingRight) 
            {
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }

            else 
            {
                transform.Translate(Vector2.left * _speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            _manager.TakeDamage();
        }
        if (other.CompareTag("BulletDestroyable"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BulletDestroyable"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

}
