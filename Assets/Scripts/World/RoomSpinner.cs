using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpinner : MonoBehaviour {

    public bool m_isSpinningLeft;
    public bool m_isSpinningRight;

    [Header("Room Spinner")]
    [Range(0f, 180f)]
    [SerializeField] float _spinAngle = 0f;
    [SerializeField] float _spinningSpeed = 0.5f;

    void Update() {
        float rotation = transform.rotation.z;

        if (m_isSpinningLeft && rotation < _spinAngle) {
            transform.Rotate(0, 0, _spinningSpeed);
        }

        else if (m_isSpinningRight && rotation > -_spinAngle) {
            transform.Rotate(0, 0, -_spinningSpeed);
        }
    }
}
