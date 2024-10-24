using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMechanism : MonoBehaviour
{
    public bool m_isEnabled { get; private set; } = false;

    public virtual void ActivateOnce(int gearRotationDirection)
    {
        _lastGearRotationDirection = gearRotationDirection;
    }
    public int _lastGearRotationDirection;

    public void Activate(int gearRotationDirection)
    {
        m_isEnabled = true;
        _lastGearRotationDirection = gearRotationDirection;
    }

    public void Disable()
    {
        m_isEnabled = false;
    }

    public void Update()
    {
        if(m_isEnabled)
        {
            ActivateOnce(_lastGearRotationDirection);
        }
    }
}
