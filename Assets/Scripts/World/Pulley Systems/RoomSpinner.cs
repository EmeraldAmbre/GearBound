using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpinner : GearMechanism
{

    [Header("Room Spinner")]
    [Range(0f, 180f)]
    [SerializeField] float _spinAngle = 0f;
    [SerializeField] float _spinningSpeed = 0.5f;
    [SerializeField] float _spinAngleToSnap = 90;
    float _angleToSnap;
    bool _isSnapping;

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        float rotation = transform.rotation.z;

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed);
            

        else if (gearRotationDirection == -1)transform.Rotate(0, 0, -_spinningSpeed);
    }

    //void Update() {
    //    float rotation = transform.rotation.z;

    //    if (m_isSpinningLeft && rotation < _spinAngle)
    //    {
    //        transform.Rotate(0, 0, _spinningSpeed);
    //    }

    //    else if (m_isSpinningRight && rotation > -_spinAngle)
    //    {
    //        transform.Rotate(0, 0, -_spinningSpeed);
    //    }

    //    else if ( !m_isSpinningLeft && !m_isSpinningRight)
    //    {
    //        if (!_isSnapping) Debug.Log("Is rotating");

    //        _isSnapping = true;

    //        //if (!_isSnapping)
    //        //{
    //        //    _isSnapping = true;
    //        //    if (transform.eulerAngles.z % _spinAngleToSnap < _spinAngleToSnap / 2) _angleToSnap = transform.eulerAngles.z - transform.eulerAngles.z % _spinAngleToSnap;
    //        //    else _angleToSnap = transform.eulerAngles.z + (_spinAngleToSnap - transform.eulerAngles.z % _spinAngleToSnap);
    //        //    Debug.Log("Current rotation : " + transform.eulerAngles.z + "    rotation to reach : " + _spinAngleToSnap);
    //        //}


    //        //if (transform.eulerAngles.z % _spinAngleToSnap != 0)
    //        //{
    //        //    if (transform.eulerAngles.z % _spinAngleToSnap > _spinAngleToSnap / 2)
    //        //    {
    //        //        if(transform.eulerAngles.z > _angleToSnap)
    //        //        {
    //        //            transform.localRotation.Set(0,0,_angleToSnap,0);
    //        //            _isSnapping = false;
    //        //        }
    //        //        else transform.Rotate(0, 0, _spinningSpeed);
    //        //    }
    //        //    else
    //        //    {
    //        //        if (transform.eulerAngles.z < _angleToSnap)
    //        //        {
    //        //            transform.localRotation.Set(0, 0, _angleToSnap, 0);
    //        //            _isSnapping = false;
    //        //        }
    //        //        else transform.Rotate(0, 0, _spinningSpeed);
    //        //    }
    //        //}

    //    }
    //    else if (_isSnapping) _isSnapping = false; 
    //}
}
