using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWheel : MonoBehaviour {

    public bool m_isSpinningLeft;
    public bool m_isSpinningRight;

    [SerializeField] float _spinningSpeed = 2f;
    [SerializeField] Transform[] _platforms;

    void Update() {

        if (m_isSpinningLeft) transform.Rotate(0, 0, _spinningSpeed);

        else if (m_isSpinningRight) transform.Rotate(0, 0, -_spinningSpeed);

        foreach (Transform plat in _platforms) {
            plat.rotation = Quaternion.identity;
        }

    }
}
