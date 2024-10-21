using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour {

    public static CheckpointManager instance;
    public string m_lastCheckpointScene;
    public Vector3 m_lastCheckpointPosition;

    void Awake() {
        if (instance == null) {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    public void SaveLastCheckpoint(string sceneName, Vector3 checkpointPosition) {
        m_lastCheckpointScene = sceneName;
        m_lastCheckpointPosition = checkpointPosition;
    }
}
