using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    GameObject _player;
    PlayerManager _manager;
    [SerializeField] AudioClip _sfxLifeUpgrade;

    void Awake() {
        _player = GameObject.FindWithTag("Player");
        _manager = _player.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _manager.LifeUpgrade();
            gameObject.SetActive(false);

            AudioManager.Instance.PlaySfx(_sfxLifeUpgrade, 9);
        }
    }
}
