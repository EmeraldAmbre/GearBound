using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float m_playerLife { get; private set; }

    public bool m_isInteracting;
    public float m_rotationInversion { get; private set; }

    void Awake() {
        m_isInteracting = false;
        m_rotationInversion = 1;
    }

    public void RotationInversion() {
        m_rotationInversion *= -1;
    }

}
