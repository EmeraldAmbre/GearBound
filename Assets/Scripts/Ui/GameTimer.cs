using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {

    private static GameTimer instance;
    private float _elapsedTime = 0f;
    private bool _isTiming = false;

    void Awake() {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        _isTiming = true;
    }

    void Update() {
        if (_isTiming) {
            _elapsedTime += Time.deltaTime;
            // Enregistrer le temps chaque seconde dans les PlayerPrefs
            if (Mathf.FloorToInt(_elapsedTime) % 1 == 0) {
                SaveTime();
            }
        }
    }

    private void SaveTime() {
        PlayerPrefs.SetFloat("GameTime", _elapsedTime);
        PlayerPrefs.Save();
    }

    public void StopTimer()
    {
        _isTiming = false;
        SaveTime();
    }

    public void ResetTimer() {
        _elapsedTime = 0f;
        PlayerPrefs.SetFloat("GameTime", _elapsedTime);
        PlayerPrefs.Save();
    }

    public float GetElapsedTime()
    {
        return _elapsedTime;
    }
}
