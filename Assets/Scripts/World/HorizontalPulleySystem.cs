using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPulleySystem : MonoBehaviour {

    public bool m_isMovingLeft;
    public bool m_isMovingRight;

    [SerializeField] float _maxLeft = 2f;
    [SerializeField] float _maxRight = 2f;
    [SerializeField] float _pulleySpeed = 0.2f;

    float currentPosX;

    void Start() {

        currentPosX = transform.position.x;
        _maxLeft = currentPosX - _maxLeft;
        _maxRight = currentPosX + _maxRight;

    }

    void Update() {

        currentPosX = transform.position.x;

        if (m_isMovingLeft) {
            if (currentPosX > _maxLeft) transform.Translate(Vector2.left * _pulleySpeed);
        }

        else if (m_isMovingRight) {
            if (currentPosX < _maxRight) transform.Translate(Vector2.right * _pulleySpeed);
        }

    }
}
