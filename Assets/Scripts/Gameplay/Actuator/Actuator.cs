using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuator : MonoBehaviour
{
    [SerializeField] Mechanism _mechanismtoTrigger;

    [SerializeField] protected bool _isActivated = false;
    [SerializeField] protected bool _isOnceActivable = false;

    virtual public void ActivateMechanism()
    {
        _mechanismtoTrigger.Activate();
        _isActivated = true;
    }

    virtual public void DeactivateMechanism()
    {
        _mechanismtoTrigger.Deactivate();
        _isActivated = false;
    }
}
