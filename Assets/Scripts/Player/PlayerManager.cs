using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    #region Public Variables

    public bool m_isInteracting { get; set; }
    public float m_playerLife { get; private set; }
    public float m_rotationInversion { get; private set; }

    #endregion

    private void Awake() {
        m_isInteracting = false;
        m_rotationInversion = 1;
    }

    public void RotationInversion() {
        m_rotationInversion *= -1;
    }

    public void TakeDamage(float damage) {
        m_playerLife -= damage;
    }

    public void Heal(float heal) {
        m_playerLife += heal;
    }
}
