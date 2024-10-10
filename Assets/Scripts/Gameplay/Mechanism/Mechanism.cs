using UnityEngine;


public class Mechanism : MonoBehaviour
{
    [SerializeField] protected bool _isActivated = false;
    [SerializeField] protected bool _isOnceActivable = false;

    virtual public void Activate()
    {
        _isActivated = true;
    }

    // Activate() and Deactivate() methods should be called always with base; 
    // Inside the overrided method of the children class
    virtual public void Deactivate()
    {
        if (!_isOnceActivable) _isActivated = false;
    }

    public void ToogleActivation()
    {
        if (!(_isOnceActivable && _isActivated)) _isActivated = !_isActivated;

        if (_isActivated) Activate();
        else Deactivate();
    }

}
