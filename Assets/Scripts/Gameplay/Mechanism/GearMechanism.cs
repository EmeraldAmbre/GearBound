using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GearMechanism
{
    public abstract void Activate(int gearRotationDirection);

    public abstract void DeActivate();

}
