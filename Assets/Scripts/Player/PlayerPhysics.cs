using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    public bool m_groundCheck = false;

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.layer == 6) m_groundCheck = true;
        
    }

    void OnTriggerExit2D(Collider2D other) {

        if (other.gameObject.layer == 6) m_groundCheck = false;

    }
}
