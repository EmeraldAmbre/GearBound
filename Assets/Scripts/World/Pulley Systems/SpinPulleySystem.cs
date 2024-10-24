using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPulleySystem :  GearMechanism
{

    [SerializeField] float _spinningSpeed = 2f;

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed);
        else if (gearRotationDirection == -1) transform.Rotate(0, 0, -_spinningSpeed);


    }

    //void Update() {

    //    if (m_isSpinningLeft) transform.Rotate(0, 0, _spinningSpeed);

    //    else if (m_isSpinningRight) transform.Rotate(0, 0, -_spinningSpeed);
        
    //}
}
