using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMechanism : MonoBehaviour
{
    public bool m_isEnabled { get; private set; } = false;
    [HideInInspector] public bool m_isPlayerInteracting = false;

    public virtual void ActivateOnce(int gearRotationDirection )
    {
        _lastGearRotationDirection = gearRotationDirection;
        if(!m_isPlayerInteracting) m_isPlayerInteracting = true;
    }

    public virtual void ActivateOnce(int gearRotationDirection ,float gearRotationScale)
    {
        _lastGearRotationDirection = gearRotationDirection;
        if (!m_isPlayerInteracting) m_isPlayerInteracting = true;
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

    public virtual void Update()
    {
        if(m_isEnabled)
        {
            ActivateOnce(_lastGearRotationDirection);
        }
    }

}
