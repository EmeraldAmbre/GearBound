using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeSystem : GearMechanism
{

    [SerializeField] float _minPivot = 0f;
    [SerializeField] float _maxPivot = 75f;
    [SerializeField] float _rotationSpeed = 0.05f;

    float _currentPivot;

    [SerializeField] Transform _pivotTransform;

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        if (gearRotationDirection == 1)
            if (_currentPivot < _maxPivot)
            {
                _currentPivot += _rotationSpeed;
                _pivotTransform.Rotate(new(0, 0, _rotationSpeed));
            }
        else if (gearRotationDirection == -1)
            if (_currentPivot > _minPivot)
            {
                _currentPivot -= _rotationSpeed;
                _pivotTransform.Rotate(new(0, 0, -_rotationSpeed));
            }
    }

    void Start() {

        _currentPivot = _pivotTransform.rotation.z;

        if (_currentPivot < _minPivot) {
            _currentPivot = _minPivot;
            _pivotTransform.rotation.Set(0, 0, _currentPivot, 0);
        }
        else if (_currentPivot > _maxPivot) {
            _currentPivot = _maxPivot;
            _pivotTransform.rotation.Set(0, 0, _currentPivot, 0);
        }

    }

    //void Update() {

    //    if (m_isMovingDown && _currentPivot > _minPivot) {
    //        _currentPivot -= _rotationSpeed;
    //        _pivotTransform.Rotate(new(0, 0, -_rotationSpeed));
    //    }

    //    else if (m_isMovingUp && _currentPivot < _maxPivot) {
    //        _currentPivot += _rotationSpeed;
    //        _pivotTransform.Rotate(new(0, 0, _rotationSpeed));
    //    }
        
    //}
}
