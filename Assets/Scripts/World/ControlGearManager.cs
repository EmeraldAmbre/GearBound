using System.Collections;
using UnityEngine;

public class ControlGearManager : MonoBehaviour {

    [SerializeField] LayerMask _detectionMask;
    Rigidbody2D _rb;
    float _initialMass;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _initialMass = _rb.mass;
    }

}