using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPulleySystem : MonoBehaviour {

    public bool m_isSpinningLeft;
    public bool m_isSpinningRight;

    [SerializeField] float _spinningSpeed = 2f;

    void Update() {

        if (m_isSpinningLeft) transform.Rotate(0, 0, _spinningSpeed);

        else if (m_isSpinningRight) transform.Rotate(0, 0, -_spinningSpeed);
        
    }
}
