using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour {

    [SerializeField] string _sceneName;

    void Start() {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            CheckpointManager.instance.SaveLastCheckpoint(_sceneName, transform.position);
        }
    }

}
