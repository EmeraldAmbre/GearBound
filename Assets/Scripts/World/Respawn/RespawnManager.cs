using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour {

    [SerializeField] GameObject _playerPackage;
    [SerializeField] GameObject _playerPackagePrefab;

    string _sceneName;
    PlayerManager _player;

    void Awake() {
        _sceneName = SceneManager.GetActiveScene().name;
        _player = FindAnyObjectByType(typeof(PlayerManager)) as PlayerManager;
    }

    void Update() {
        if (_player.m_playerLife <= 0) Death();
    }

    void Death() {
        if (PlayerPrefs.GetString("sceneName").Equals(_sceneName)) {
            float x = PlayerPrefs.GetFloat(_sceneName + "_x");
            float y = PlayerPrefs.GetFloat(_sceneName + "_y");
            float z = PlayerPrefs.GetFloat(_sceneName + "_z");
            Vector3 respawnPosition = new (x, y, z);
            Destroy(_playerPackage);
            Instantiate(_playerPackagePrefab, respawnPosition, Quaternion.identity);
        }
    }
}
