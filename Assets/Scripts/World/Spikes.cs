using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    GameObject _player;
    PlayerManager _manager;
    void Start() {
        _player = GameObject.FindWithTag("Player");
        _manager = _player.GetComponent<PlayerManager>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
         _manager.TakeDamage();
    }
}
