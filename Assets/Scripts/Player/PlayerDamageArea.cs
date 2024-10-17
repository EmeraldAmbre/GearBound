using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageArea : MonoBehaviour
{
    [SerializeField] PlayerManager _playerManager;

    private void OnTriggerStay2D(Collider2D collision)
    {
        _playerManager.TakeDamage();
    }
}
