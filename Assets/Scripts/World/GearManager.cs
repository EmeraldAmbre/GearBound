using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    // Allow gears to spin or not, by detecting where's the player and if it can interact
    [Header("Non optionnal settings")]
    [SerializeField] bool _isActivableByOtherGears; // Check it in editor if u want that this gear can spin with player
    [SerializeField] bool _isReversingRotationEffectOnMechanism; 

    // Linked pulleys and linked interactions
    // Drag and drop your linked item(s) in editor
    [Header("Linked Items")]
    [SerializeField] GearMechanism _gearMechanismToActivate;
    [SerializeField] bool _isActivatingDifferentMechanismByDirection = false;
    [SerializeField] GearMechanism _leftGearMechanismToActivate;
    [SerializeField] GearMechanism _rightGearMechanismToActivate;
    [SerializeField] bool _isActivatingSeveralMechanism = false;
    [SerializeField] List<GearMechanism> _listSeveralMechanismm = new List<GearMechanism>();

    bool _isPlayerNear;
    PlayerController _player;
    Rigidbody2D _gearRigidbody;


    [Header("Effect")]
    [SerializeField] AudioClip _sfxActivation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _isPlayerNear = true;
            _player = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isPlayerNear = false;
            _player = null;

            AudioManager.Instance.StopSfxLoop(); 
            ResetGearMechanisms();
        }
    }


    void Start() 
    {
        _gearRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        if (_isPlayerNear == true && _player != null)
        {
            if (_player.m_inputX != 0)
            {
                AudioManager.Instance.PlaySfxLoop(_sfxActivation);


                int directionRotationUpgrade = _player.m_rotationInversion ? -1 : 1;

                _gearRigidbody.freezeRotation = false;

                int gearPlayerRotationDirection = _player.m_currentGearRotation > 0 ? 1 : -1;
                if(_isReversingRotationEffectOnMechanism) gearPlayerRotationDirection *= -1;
                gearPlayerRotationDirection *= directionRotationUpgrade;

                if (!_isActivatingDifferentMechanismByDirection && !_isActivatingSeveralMechanism && _gearMechanismToActivate != null)
                {
                    _gearMechanismToActivate.ActivateOnce(gearPlayerRotationDirection , _player.m_gearRotationDashMultiplier);
                }
                else if (_isActivatingDifferentMechanismByDirection)
                {
                    if (gearPlayerRotationDirection == -1 && _leftGearMechanismToActivate != null) _leftGearMechanismToActivate.ActivateOnce(gearPlayerRotationDirection , _player.m_gearRotationDashMultiplier);
                    else if (_rightGearMechanismToActivate != null) _rightGearMechanismToActivate.ActivateOnce(gearPlayerRotationDirection , _player.m_gearRotationDashMultiplier);
                }
                else if(_isActivatingSeveralMechanism)
                {
                    foreach(GearMechanism gearMechanism in _listSeveralMechanismm)
                    {
                        if (gearPlayerRotationDirection == -1) gearMechanism.ActivateOnce(gearPlayerRotationDirection, _player.m_gearRotationDashMultiplier);
                        else gearMechanism.ActivateOnce(gearPlayerRotationDirection, _player.m_gearRotationDashMultiplier);
                    }
                }
            }
            else
            {
                ResetGearMechanisms();

                AudioManager.Instance.StopSfxLoop();
            }
        }
        else
        {
            if (_isActivableByOtherGears == false) _gearRigidbody.freezeRotation = true;
            ResetGearMechanisms();
        }

    }

    void ResetGearMechanisms()
    {
        if (!_isActivatingDifferentMechanismByDirection && !_isActivatingSeveralMechanism && _gearMechanismToActivate != null)
        {
            if(_gearMechanismToActivate.m_isPlayerInteracting) _gearMechanismToActivate.m_isPlayerInteracting = false;
        }
        else if (_isActivatingDifferentMechanismByDirection && !_isActivatingSeveralMechanism)
        {
            if(_leftGearMechanismToActivate != null )
            {

                if (_leftGearMechanismToActivate.m_isPlayerInteracting )
                {
                    _leftGearMechanismToActivate.m_isPlayerInteracting = false;
                }
            }
            if(_rightGearMechanismToActivate != null)
            {

                if ( _rightGearMechanismToActivate.m_isPlayerInteracting)
                {
                    _rightGearMechanismToActivate.m_isPlayerInteracting = false;
                }
            }
        }
        else if (_isActivatingSeveralMechanism)
        {
            foreach (GearMechanism gearMechanism in _listSeveralMechanismm)
            {
                if (gearMechanism.m_isPlayerInteracting) gearMechanism.m_isPlayerInteracting = false;
            }
        }
    }
}