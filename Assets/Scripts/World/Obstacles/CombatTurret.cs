using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTurret : MonoBehaviour {

    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _shootingPoint;
    [SerializeField] float _shootLoopDuration = 2f;

    float _shootTimer;
    bool _facingRight;

    void Start() {
        _shootTimer = _shootLoopDuration;
    }

    void Update() {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0) {
            _shootTimer = _shootLoopDuration;
            GameObject bullet = Instantiate(_bulletPrefab, _shootingPoint.transform.position, _shootingPoint.transform.rotation);
            BulletManager bulletManager = bullet.GetComponent<BulletManager>();
            _facingRight = transform.localScale.x == 1 ? true : false;
            bulletManager.m_isGoingRight = _facingRight;
            Destroy(bullet, bulletManager.m_lifeTime);
        }
    }
}
