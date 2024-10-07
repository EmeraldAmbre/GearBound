using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] Rigidbody2D _playerRigidbody;

    void Start() {

        if (_playerRigidbody == null) { _playerRigidbody = GetComponentInParent<Rigidbody2D>(); }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
