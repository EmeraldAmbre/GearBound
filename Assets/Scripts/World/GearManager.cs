using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour {

    // Allow gears to spin or not, by detecting where's the player and if it can interact
    [Header("Non optionnal settings")]
    [SerializeField] bool _isActivableByOtherGears; // Check it in editor if u want that this gear can spin with player

    // Linked pulleys and linked interactions
    // Drag and drop your linked item(s) in editor
    [Header("Linked Items")]
    [SerializeField] GearMechanism _gearMechanismToActivate;
    [SerializeField] bool _isActivatingDifferentMechanismByDirection = false;
    [SerializeField] GearMechanism _leftGearMechanismToActivate;
    [SerializeField] GearMechanism _rightGearMechanismToActivate;


    bool _isPlayerNear;
    PlayerController _player;
    Rigidbody2D _gearRigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _isPlayerNear = true;
            _player = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isPlayerNear = false;
            _player = null;
        }
    }


    void Start() {
        _gearRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {

        if (_isPlayerNear == true && _player != null)
        {
            if (_player.m_inputX != 0)
            {
                _gearRigidbody.freezeRotation = false;

                int gearPlayerRotationDirection = _player.m_currentGearRotation > 0 ? 1 : -1;
                if (!_isActivatingDifferentMechanismByDirection && _gearMechanismToActivate != null)
                {
                    _gearMechanismToActivate.ActivateOnce(gearPlayerRotationDirection);
                }
                else if (_isActivatingDifferentMechanismByDirection)
                {
                    if (gearPlayerRotationDirection == -1) _leftGearMechanismToActivate.Activate(gearPlayerRotationDirection);
                    else _rightGearMechanismToActivate.ActivateOnce(gearPlayerRotationDirection);
                }
            }
            else
            {
                ResetGearMechanisms();
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
        if (!_isActivatingDifferentMechanismByDirection && _gearMechanismToActivate != null)
        {
            if(_gearMechanismToActivate.m_isPlayerInteracting) _gearMechanismToActivate.m_isPlayerInteracting = false;
        }
        else if (_isActivatingDifferentMechanismByDirection)
        {
            if (_leftGearMechanismToActivate.m_isPlayerInteracting || _rightGearMechanismToActivate.m_isPlayerInteracting)
            {
                _leftGearMechanismToActivate.m_isPlayerInteracting = false;
                _rightGearMechanismToActivate.m_isPlayerInteracting = false;
            }
        }
    }
}