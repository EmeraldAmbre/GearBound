using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMap : MonoBehaviour {

    [SerializeField] Transform _player;  // Référence au joueur réel dans la scène
    [SerializeField] RectTransform _miniMapRect;  // RectTransform de la mini-map (image de fond)
    [SerializeField] RectTransform _playerMarker;  // RectTransform du minisprite (point du joueur)

    [HideInInspector]
    [SerializeField] Vector2 _mapWorldSize = new Vector2(100, 100);
    [SerializeField] Vector2 _miniMapSize = new Vector2(500, 500);

    [Header("Offset Settings")]
    [SerializeField] Vector2 _offsetRoom1 = new Vector2(1435, 350);
    [SerializeField] Vector2 _offsetRoom2 = new Vector2(1160, -270);
    [SerializeField] Vector2 _offsetRoom3 = new Vector2(1210, 350);
    [SerializeField] Vector2 _offsetRoom4 = new Vector2(0, 500);
    [SerializeField] Vector2 _offsetRoom5 = new Vector2(10, 10);

    [Header("Rooms")]
    [SerializeField] GameObject _room1;
    [SerializeField] GameObject _room2;
    [SerializeField] GameObject _room3;
    [SerializeField] GameObject _room4;
    [SerializeField] GameObject _room5;

    [Header("Minimap Player Sprite")]
    [SerializeField] Image _playerImage;

    bool _isMapActive;
    string _sceneName;
    Vector2 _mainOffset;
    PlayerInputAction _input;

    void Start() {
        InitInput();
        _isMapActive = false;
    }

    void Update() {
        if (_isMapActive) {
            UpdatePlayerMarkerPosition();
        }

        else {
            _playerMarker.anchoredPosition = new Vector2(99999f, 99999f);
        }
    }

    void OnDestroy() {
        _input.Player.Map.performed -= OnPerformMap;
        _input.Player.Disable();
    }

    void InitInput() {
        _input = new();
        _input.Player.Map.performed += OnPerformMap;
        _input.Enable();
    }

    void OnPerformMap(InputAction.CallbackContext context) {

        VisitedScene();

        int index = PlayerPrefs.GetInt("visited_room");

        if (_isMapActive is false) {

            switch (_sceneName) {

                case "Room 1":
                    _mainOffset = _offsetRoom1;
                    break;

                case "Room 2":
                    _mainOffset = _offsetRoom2;
                    break;

                case "Room 3":
                    _mainOffset = _offsetRoom3;
                    break;

                case "Room 4":
                    _mainOffset = _offsetRoom4;
                    break;

                case "Room 5":
                    _mainOffset = _offsetRoom5;
                    break;
            }

            switch (index) {

                case 1:
                    _room1.SetActive(true);
                    break;
                case 2:
                    _room2.SetActive(true);
                    break;
                case 3:
                    _room3.SetActive(true);
                    break;
                case 4:
                    _room4.SetActive(true);
                    break;
                case 5:
                    _room5.SetActive(true);
                    break;
            }

            _isMapActive = true;

        }

        else {
            _isMapActive = false;
            _room1.SetActive(false);
            _room2.SetActive(false);
            _room3.SetActive(false);
            _room4.SetActive(false);
            _room5.SetActive(false);
        }
    }
    
    void VisitedScene() {
        _sceneName = SceneManager.GetActiveScene().name;
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
        }
    }

    void UpdatePlayerMarkerPosition() {

        // Calculer la position du joueur dans le monde en fonction de la taille de la carte
        Vector2 playerPosWorld = new Vector2(_player.position.x, _player.position.y);

        // Mapper la position du joueur sur la mini-map
        float xRatio = playerPosWorld.x / _mapWorldSize.x;
        float yRatio = playerPosWorld.y / _mapWorldSize.y;

        // Calculer la position du marqueur sur la mini-map
        float xPos = (xRatio * _miniMapSize.x - _miniMapSize.x / 2) + _mainOffset.x;
        float yPos = (yRatio * _miniMapSize.y - _miniMapSize.y / 2) + _mainOffset.y;

        // Mettre à jour la position du minisprite du joueur
        _playerMarker.anchoredPosition = new Vector2(xPos, yPos);
    }

}
