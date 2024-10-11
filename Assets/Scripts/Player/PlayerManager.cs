using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float m_playerLife { get; private set; }

    public bool m_isInteracting;
    public bool m_rotationInversion;

    void Awake() {
        m_isInteracting = false;
        m_rotationInversion = false;
    }

    public void RotationInversion(HingeJoint2D hingeJoint) {
        m_rotationInversion = !m_rotationInversion;
        JointMotor2D moteur = hingeJoint.motor;
        moteur.motorSpeed = -moteur.motorSpeed;
        hingeJoint.motor = moteur;
    }

}
