using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    [SerializeField] bool _isOneShot = true;
    [SerializeField] Vector3 _finalPos;
    [SerializeField] float _speed = 2f;

    bool _isOnPlate = false;
    Vector3 _initialPos;

    public bool m_isActivate;

    #region Unity API
    void Start() {
        _initialPos = transform.position;
        m_isActivate = false;
    }

    void Update() {
        if (_isOnPlate) DescendrePlaque();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _isOnPlate = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _isOnPlate = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _isOnPlate = false;
            if (!m_isActivate) RemonterPlaque();
            else if (m_isActivate && _isOneShot == false) RemonterPlaque();
        }
    }
    #endregion

    void DescendrePlaque() {

        transform.position = Vector3.MoveTowards(transform.position, _finalPos, _speed * Time.deltaTime);

        // Arrête la descente quand la plaque atteint la position finale
        if (transform.position == _finalPos) {
            _isOnPlate = false;
            m_isActivate = true;
        }
    }

    void RemonterPlaque() {
        transform.position = _initialPos;
    }
}
