using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Map : MonoBehaviour {

    [Header("Mini map")]
    [SerializeField] GameObject[] _miniMap_room_1;
    [SerializeField] GameObject[] _miniMap_room_2;
    [SerializeField] GameObject[] _miniMap_room_3;
    [SerializeField] GameObject[] _miniMap_room_4;
    [SerializeField] GameObject[] _miniMap_room_5;
    [SerializeField] GameObject[] _miniMap_room_6;
    [SerializeField] GameObject[] _miniMap_room_7;

    [Header("Fullscreen map")]
    [SerializeField] GameObject[] _fullscreenMap_room_1;
    [SerializeField] GameObject[] _fullscreenMap_room_2;
    [SerializeField] GameObject[] _fullscreenMap_room_3;
    [SerializeField] GameObject[] _fullscreenMap_room_4;
    [SerializeField] GameObject[] _fullscreenMap_room_5;
    [SerializeField] GameObject[] _fullscreenMap_room_6;
    [SerializeField] GameObject[] _fullscreenMap_room_7;

    PlayerInputAction _input;

    [SerializeField] int _mapMode = 0;
    string _sceneName;

    void InitInput() {
        _input = new();
        _input.Player.Map.performed += OnPerformMap;
        _input.Enable();
    }

    void Start() {
        InitInput();
    }

    void OnDestroy() {
        _input.Player.Map.performed -= OnPerformMap;
        _input.Player.Disable();
    }

    void VisitedScene() {
        switch (_sceneName) {
            case "Room 1":
                if (PlayerPrefs.HasKey("visited_room") is false) {
                    PlayerPrefs.SetInt("visited_room", 1);
                    PlayerPrefs.Save();
                } break;
            case "Room 2":
                if (PlayerPrefs.GetInt("visited_room") < 2) {
                    PlayerPrefs.SetInt("visited_room", 2);
                    PlayerPrefs.Save();
                } break;
            case "Room 3":
                if (PlayerPrefs.GetInt("visited_room") < 3) {
                    PlayerPrefs.SetInt("visited_room", 3);
                    PlayerPrefs.Save();
                } break;
            case "Room 4":
                if (PlayerPrefs.GetInt("visited_room") < 4) {
                    PlayerPrefs.SetInt("visited_room", 4);
                    PlayerPrefs.Save();
                } break;
            case "Room 5":
                if (PlayerPrefs.GetInt("visited_room") < 5) {
                    PlayerPrefs.SetInt("visited_room", 5);
                    PlayerPrefs.Save();
                } break;
            case "Room 6":
                if (PlayerPrefs.GetInt("visited_room") < 6) {
                    PlayerPrefs.SetInt("visited_room", 6);
                    PlayerPrefs.Save();
                } break;
            case "Room 7":
                if (PlayerPrefs.GetInt("visited_room") < 7) {
                    PlayerPrefs.SetInt("visited_room", 7);
                    PlayerPrefs.Save();
                } break;
        }
    }

    void OnPerformMap(InputAction.CallbackContext context) {
        VisitedScene();
        _sceneName = SceneManager.GetActiveScene().name;
        int index = PlayerPrefs.GetInt("visited_room");

        if (_mapMode == 0) {
            _mapMode = 1;
            switch (_sceneName) {
                case "Room 1":
                    _fullscreenMap_room_1[index-1].SetActive(true);
                    break;
                case "Room 2":
                    _fullscreenMap_room_2[index-2].SetActive(true);
                    break;
                case "Room 3":
                    _fullscreenMap_room_3[index - 3].SetActive(true);
                    break;
                case "Room 4":
                    _fullscreenMap_room_4[index - 4].SetActive(true);
                    break;
                case "Room 5":
                    _fullscreenMap_room_5[index - 5].SetActive(true);
                    break;
                case "Room 6":
                    _fullscreenMap_room_6[index - 6].SetActive(true);
                    break;
                case "Room 7":
                    _fullscreenMap_room_7[0].SetActive(true);
                    break;
            }
        }

        else if (_mapMode == 1) {
            _mapMode = 2;
            foreach (var item in _fullscreenMap_room_1) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_2) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_3) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_4) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_5) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_6) item.SetActive(false);
            foreach (var item in _fullscreenMap_room_7) item.SetActive(false);
            switch (_sceneName) {
                case "Room 1":
                    _miniMap_room_1[index - 1].SetActive(true);
                    break;
                case "Room 2":
                    _miniMap_room_2[index - 2].SetActive(true);
                    break;
                case "Room 3":
                    _miniMap_room_3[index - 3].SetActive(true);
                    break;
                case "Room 4":
                    _miniMap_room_4[index - 4].SetActive(true);
                    break;
                case "Room 5":
                    _miniMap_room_5[index - 5].SetActive(true);
                    break;
                case "Room 6":
                    _miniMap_room_6[index - 6].SetActive(true);
                    break;
                case "Room 7":
                    _miniMap_room_7[0].SetActive(true);
                    break;
            }
        }

        else if (_mapMode == 2) {
            _mapMode = 0;
            foreach (var item in _miniMap_room_1) item.SetActive(false);
            foreach (var item in _miniMap_room_2) item.SetActive(false);
            foreach (var item in _miniMap_room_3) item.SetActive(false);
            foreach (var item in _miniMap_room_4) item.SetActive(false);
            foreach (var item in _miniMap_room_5) item.SetActive(false);
            foreach (var item in _miniMap_room_6) item.SetActive(false);
            foreach (var item in _miniMap_room_7) item.SetActive(false);
        }
    }
}
