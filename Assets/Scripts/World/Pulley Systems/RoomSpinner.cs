using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpinner : GearMechanism
{

    [Header("Room Spinner")]
    [Range(0f, 180f)]
    [SerializeField] float _spinningSpeed = 40f;
    [SerializeField] float _snapingSnapSpeed = 20f;
    [SerializeField] float _spinAngleToSnap = 90;
    float _angleToSnap;

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);
        float rotation = transform.rotation.z;

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed * Time.deltaTime);
        else if (gearRotationDirection == -1)transform.Rotate(0, 0, -_spinningSpeed * Time.deltaTime);
    }

    public override void ActivateOnce(int gearRotationDirection, float gearRotationScale)
    {
        base.ActivateOnce(gearRotationDirection , gearRotationScale);
        float rotation = transform.rotation.z;

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed * gearRotationScale * Time.deltaTime);
        else if (gearRotationDirection == -1) transform.Rotate(0, 0, -_spinningSpeed * gearRotationScale * Time.deltaTime);
    }

 

    public void LateUpdate()
    {

        if (!m_isPlayerInteracting && (transform.eulerAngles.z % _spinAngleToSnap != 0))
        {
            if (transform.eulerAngles.z % _spinAngleToSnap < _spinAngleToSnap / 2)
            {
                _angleToSnap = transform.eulerAngles.z - transform.eulerAngles.z % _spinAngleToSnap;

                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0, 0, _angleToSnap);
                transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, _snapingSnapSpeed * Time.deltaTime);



            }
            else 
            {
                _angleToSnap = transform.eulerAngles.z + (_spinAngleToSnap - transform.eulerAngles.z % _spinAngleToSnap);

                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0, 0, _angleToSnap);
                transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, _snapingSnapSpeed * Time.deltaTime);
            }
        }

     

            //if (!_isSnapping)
            //{
            //    _isSnapping = true;
            //    if (transform.eulerAngles.z % _spinAngleToSnap < _spinAngleToSnap / 2) _angleToSnap = transform.eulerAngles.z - transform.eulerAngles.z % _spinAngleToSnap;
            //    else _angleToSnap = transform.eulerAngles.z + (_spinAngleToSnap - transform.eulerAngles.z % _spinAngleToSnap);
            //    Debug.Log("Current rotation : " + transform.eulerAngles.z + "    rotation to reach : " + _spinAngleToSnap);
            //}


            //if (transform.eulerAngles.z % _spinAngleToSnap != 0)
            //{
            //    if (transform.eulerAngles.z % _spinAngleToSnap > _spinAngleToSnap / 2)
            //    {
            //        if(transform.eulerAngles.z > _angleToSnap)
            //        {
            //            transform.localRotation.Set(0,0,_angleToSnap,0);
            //            _isSnapping = false;
            //        }
            //        else transform.Rotate(0, 0, _spinningSpeed);
            //    }
            //    else
            //    {
            //        if (transform.eulerAngles.z < _angleToSnap)
            //        {
            //            transform.localRotation.Set(0, 0, _angleToSnap, 0);
            //            _isSnapping = false;
            //        }
            //        else transform.Rotate(0, 0, _spinningSpeed);
            //    }
            //}
    }
}
