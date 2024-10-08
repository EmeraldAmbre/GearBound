using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _offset = new Vector3(0, 2, -10);
    [SerializeField] float _speed = 20f;
    [SerializeField] bool _cameraLocked;

    void Start() {
        _cameraLocked = true;
    }

    void Update() {
        if (Input.GetKey(KeyCode.K)) _cameraLocked = !_cameraLocked;
    }

    void LateUpdate() {

        if (_player != null && _cameraLocked) {

            Vector3 positionCible = _player.position + _offset;
            transform.position = Vector3.Lerp(transform.position, positionCible, _speed * Time.deltaTime);

        }
    }

}
