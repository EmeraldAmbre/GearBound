using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPulleySystem :  GearMechanism
{

    [SerializeField] float _spinningSpeed = 2f;

    public override void ActivateOnce(int gearRotationDirection)
    {
        base.ActivateOnce(gearRotationDirection);

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed * Time.deltaTime);
        else if (gearRotationDirection == -1) transform.Rotate(0, 0, -_spinningSpeed * Time.deltaTime);


    }

    public override void ActivateOnce(int gearRotationDirection , float gearRotationScale = 1)
    {
        base.ActivateOnce(gearRotationDirection , gearRotationScale);

        if (gearRotationDirection == 1) transform.Rotate(0, 0, _spinningSpeed * gearRotationScale * Time.deltaTime);
        else if (gearRotationDirection == -1) transform.Rotate(0, 0, -_spinningSpeed * gearRotationScale * Time.deltaTime);


    }
}
