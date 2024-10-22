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
            float x = transform.position.x; PlayerPrefs.SetFloat("checkpoint_pos_x", x);
            float y = transform.position.y; PlayerPrefs.SetFloat("checkpoint_pos_y", y);
            float z = transform.position.z; PlayerPrefs.SetFloat("checkpoint_pos_z", z);
            PlayerPrefs.Save();
        }
    }

}
