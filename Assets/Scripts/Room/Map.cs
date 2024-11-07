using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Map : MonoBehaviour {

    [SerializeField] GameObject _miniMap;
    [SerializeField] GameObject[] _redspotsMinimap;
    [SerializeField] GameObject _map;
    [SerializeField] GameObject[] _redspotsMap;

    PlayerInputAction _input;

    bool _isMinimapEnable = false;
    bool _isMapEnable = false;
    string _sceneName;

    void InitInput() {
        _input = new();
        _input.Player.Map.performed += OnPerformMap;
        _input.Enable();
    }

    void Start() {
        InitInput();
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void OnDestroy() {
        _input.Player.CloseBoxText.performed -= OnPerformMap;
        _input.Player.Disable();
    }

    void OnPerformMap(InputAction.CallbackContext context) {

        if (_isMapEnable) {
            _isMapEnable = false;
            _map.SetActive(false);
            foreach (var redspot in _redspotsMap) { redspot.SetActive(false); }
        }

        else if (_isMinimapEnable) {
            _isMinimapEnable = false;
            _isMapEnable = true;
            _map.SetActive(true);
            _miniMap.SetActive(false);
            foreach (var redspot in _redspotsMinimap) { redspot.SetActive(false); }
            switch (_sceneName) {
                case "Room 1":
                    _redspotsMap[0].SetActive(true);
                    break;
                case "Room 2":
                    _redspotsMap[1].SetActive(true);
                    break;
                case "Room 3":
                    _redspotsMap[2].SetActive(true);
                    break;
                case "Room 4":
                    _redspotsMap[3].SetActive(true);
                    break;
            }
        }

        else {
            _isMinimapEnable = true;
            _miniMap.SetActive(true);
            switch (_sceneName) {
                case "Room 1":
                    _redspotsMinimap[0].SetActive(true);
                    break;
                case "Room 2":
                    _redspotsMinimap[1].SetActive(true);
                    break;
                case "Room 3":
                    _redspotsMinimap[2].SetActive(true);
                    break;
                case "Room 4":
                    _redspotsMinimap[3].SetActive(true);
                    break;
            }
        }
    }
}
