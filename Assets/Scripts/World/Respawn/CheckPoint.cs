using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour {

    string _sceneName;

    private void Awake() {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.CompareTag("Player")) {
            PlayerPrefs.SetString("sceneName", _sceneName);
            PlayerPrefs.SetFloat(_sceneName + "_x", transform.position.x);
            PlayerPrefs.SetFloat(_sceneName + "_y", transform.position.y);
            PlayerPrefs.SetFloat(_sceneName + "_z", transform.position.z);
            PlayerPrefs.Save();
        }
    }

}
