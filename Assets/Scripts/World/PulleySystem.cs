using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleySystem : MonoBehaviour {

    public bool m_isMovingUp;
    public bool m_isMovingDown;

    [SerializeField] float _minHeight;
    [SerializeField] float _maxHeight;
    [SerializeField] float _pulleySpeed;

    float currentHeight;

    void Update() {

        currentHeight = transform.position.y;

        if (m_isMovingUp) {
            if (currentHeight < _maxHeight) transform.Translate(Vector2.up * _pulleySpeed);
        }

        else if (m_isMovingDown) {
            if (currentHeight > _minHeight) transform.Translate(Vector2.down * _pulleySpeed);
        }
        
    }
}
