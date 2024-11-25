using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour {

    [Header("UI settings")]
    [SerializeField] private List<GameObject> _fullHearts;
    [SerializeField] private List<GameObject> _demiHearts;
    [SerializeField] private List<GameObject> _emptyHearts;

    [Header("Invincibility frame")]
    [SerializeField] private float _invincibilityTime = 2f;
    [SerializeField] private float _invincibilityFrame = 0.1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    PlayerInputAction _input;
    PlayerController _playerController;

    #region Public Variables
    public int m_maxLife = 2;
    public int m_playerLife;
    public bool m_isInvincible { get; private set; }

    // Singleton
    public static PlayerManager instance;
    #endregion


    [Header("VFX")]
    [SerializeField] ParticleSystem _damagedParticule;
    [SerializeField] ParticleSystem _deathParticule;
    [SerializeField] Animation _animTransition;
    [SerializeField] GameObject _playerSprite;
    [SerializeField] Image _transitionImage;


    [Header("SFX")]
    [SerializeField] List<AudioClip> _listSfxPlayerDamaged;
    [SerializeField] List<AudioClip> _listSfxPlayerDeath;

    #region Unity API
    void Awake() 
    {
        //_transitionImage.color = new Color(0 ,0 ,0 ,1);
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (PlayerPrefs.HasKey("max_player_life")) m_maxLife = PlayerPrefs.GetInt("max_player_life");

        if (PlayerPrefs.HasKey("is_changing_room")) {
            if (PlayerPrefs.GetInt("is_changing_room") == 1) {
                m_playerLife = PlayerPrefs.GetInt("current_player_life");
                PlayerPrefs.SetInt("is_changing_room", 0);
                PlayerPrefs.Save();
            }

            else {
                m_playerLife = m_maxLife;
            }
        }

        else {
            m_playerLife = m_maxLife;
        }
    }

    void Start()
    {
        _animTransition.Play("ScreenBlacktransitionOut");
        LifeUpdate();


        float x = PlayerPrefs.GetFloat("checkpoint_pos_x");
        float y = PlayerPrefs.GetFloat("checkpoint_pos_y");
        float z = PlayerPrefs.GetFloat("checkpoint_pos_z");
        bool isRespawn = false;

        if (PlayerPrefs.HasKey("is_respawn")) {
            if (PlayerPrefs.GetInt("is_respawn") == 1) isRespawn = true;
        }

        if ((x != 0 || y != 0 || z != 0) && isRespawn) {
            transform.position = new Vector3(x, y, z);
            PlayerPrefs.SetInt("is_respawn", 0);
            PlayerPrefs.Save();
            isRespawn = false;
        }

        InitInput();

        _playerController = GetComponent<PlayerController>();
    }

    void Update() {
        LifeUpdate();
    }

    void InitInput() {
        _input = new();
        _input.Player.GodModeLifeUp.performed += OnPerformGodModeLifeUp;
        _input.Player.GodModeLvl1.performed += OnPerformGodModeScene1;
        _input.Player.GodModeLvl2.performed += OnPerformGodModeScene2;
        _input.Player.GodModeLvl3.performed += OnPerformGodModeScene3;
        _input.Player.GodModeLvl4.performed += OnPerformGodModeScene4;
        _input.Player.GodModeLvl5.performed += OnPerformGodModeScene5;
        _input.Player.GodModeLvl6.performed += OnPerformGodModeScene6;
        _input.Enable();
    }

    void OnDestroy() {
        _input.Player.GodModeLifeUp.performed -= OnPerformGodModeLifeUp;
        _input.Player.GodModeLvl1.performed -= OnPerformGodModeScene1;
        _input.Player.GodModeLvl2.performed -= OnPerformGodModeScene2;
        _input.Player.GodModeLvl3.performed -= OnPerformGodModeScene3;
        _input.Player.GodModeLvl4.performed -= OnPerformGodModeScene4;
        _input.Player.GodModeLvl5.performed -= OnPerformGodModeScene5;
        _input.Player.GodModeLvl6.performed -= OnPerformGodModeScene6;
        _input.Player.Disable();
    }

    void OnPerformGodModeLifeUp(InputAction.CallbackContext context) {
        LifeUpgrade();
    }

    void OnPerformGodModeScene1(InputAction.CallbackContext context) {
        ChangeRoom();
        SceneManager.LoadScene("Room 1");
    }

    void OnPerformGodModeScene2(InputAction.CallbackContext context) {
        ChangeRoom();
        SceneManager.LoadScene("Room 2");
    }

    void OnPerformGodModeScene3(InputAction.CallbackContext context) {
        ChangeRoom();
        SceneManager.LoadScene("Room 3");
    }

    void OnPerformGodModeScene4(InputAction.CallbackContext context)
    {
        ChangeRoom();
        SceneManager.LoadScene("Room 4");
    }

    void OnPerformGodModeScene5(InputAction.CallbackContext context)
    {
        ChangeRoom();
        SceneManager.LoadScene("Room 5");
    }

    void OnPerformGodModeScene6(InputAction.CallbackContext context)
    {
        ChangeRoom();
        SceneManager.LoadScene("Room 5");
    }
    #endregion

    #region Public Methods
    public void TakeDamage(int nb_damage = 1) {
        if (!m_isInvincible) {
            if (m_playerLife > 0) {
                AudioManager.Instance.PlayRandomSfx(_listSfxPlayerDamaged, 7);
                _damagedParticule.Play();
                m_playerLife -= nb_damage;
                LifeUpdate();
                if (m_playerLife <= 0) Death();
                else StartCoroutine(Invincibility());
            }
        }
    }

    public void Heal() {
        if (m_playerLife < m_maxLife) m_playerLife += 1;
        LifeUpdate();
    }

    public void LifeUpgrade() {
        if (m_maxLife < (_fullHearts.Count * 2)) {
            m_maxLife += 2;
            m_playerLife += 2;
        }
    }

    public void ChangeRoom()
    {
        _playerController.m_isControllable = false;
        _playerController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        _animTransition.Play("ScreenBlacktransitionINQuick");
        PlayerPrefs.SetInt("is_changing_room", 1);
        PlayerPrefs.SetInt("max_player_life", m_maxLife);
        PlayerPrefs.SetInt("current_player_life", m_playerLife);
        PlayerPrefs.Save();
    }


    public void Death() 
    {

        StartCoroutine(MakeDeathTransition());
    }



    // Coroutine to wait for 2 seconds
    IEnumerator MakeDeathTransition()
    {
        _playerController.m_isControllable = false;

        _playerController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;


        AudioManager.Instance.PlayRandomSfx(_listSfxPlayerDeath, 8);
        yield return new WaitForSeconds(1.2f);

        _deathParticule.Play();
        _playerSprite.SetActive(false);

        yield return new WaitForSeconds(2f);

        _animTransition.Play("ScreenBlacktransitionIN");

        yield return new WaitForSeconds(1.9f);


        _animTransition.Stop();
        yield return new WaitForSeconds(0.1f);
        _playerSprite.SetActive(true);

        _animTransition.Play("ScreenBlacktransitionOut");


        Respawn();

    }

    private void Respawn()
    {
        PlayerPrefs.SetInt("max_player_life", m_maxLife);
        PlayerPrefs.Save();

        _playerController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        if (!string.IsNullOrEmpty(CheckpointManager.instance.m_lastCheckpointScene))
        {

            if (SceneManager.GetActiveScene().name != CheckpointManager.instance.m_lastCheckpointScene)
            {
                PlayerPrefs.SetInt("is_respawn", 1); PlayerPrefs.Save();
                SceneManager.LoadScene(CheckpointManager.instance.m_lastCheckpointScene);
                StartCoroutine(RespawnAfterLoading());
            }

            else
            {
                RespawnPlayer(CheckpointManager.instance.m_lastCheckpointPosition);
            }
        }

        else
        {
            Debug.LogWarning("Aucun checkpoint enregistré. Respawn à la position initiale.");
            // Définir ici une position par défaut si aucun checkpoint n'a été visité
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator RespawnAfterLoading() {

        RespawnPlayer(CheckpointManager.instance.m_lastCheckpointPosition);
        yield return null; // Do not delete !
    }

    private void RespawnPlayer(Vector3 respawnPosition) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            player.transform.position = respawnPosition;
            m_playerLife = m_maxLife;
            RoomData.Instance.m_isPlayerRespawning = true;
        }

        else {
            Debug.LogError("Impossible de trouver l'objet Player pour respawn.");
        }
    }

    private IEnumerator Invincibility() {

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_isInvincible = true;
        float timer = 0f;

        while (timer < _invincibilityTime) {

            _spriteRenderer.enabled = !_spriteRenderer.enabled;

            yield return new WaitForSeconds(_invincibilityFrame);

            timer += _invincibilityFrame;
        }

        _spriteRenderer.enabled = true;
        m_isInvincible = false;
    }

    private void LifeUpdate() {

        int fullHearts = m_playerLife / 2;
        bool hasDemiHeart = m_playerLife % 2 != 0;
        int emptyHearts;

        if (hasDemiHeart)  {
            
            emptyHearts = (m_maxLife/2) - fullHearts - 1;

            for (int i = 0; i < fullHearts + emptyHearts + 1; i++) {

                if (i < fullHearts) {
                    _fullHearts[i].SetActive(true);
                    _demiHearts[i].SetActive(false);
                    _emptyHearts[i].SetActive(false);
                }

                else if (i == fullHearts) {
                    _fullHearts[i].SetActive(false);
                    _demiHearts[i].SetActive(true);
                    _emptyHearts[i].SetActive(false);
                }

                else {
                    _fullHearts[i].SetActive(false);
                    _demiHearts[i].SetActive(false);
                    _emptyHearts[i].SetActive(true);
                }
            }
        }
        
        else {
            
            emptyHearts = (m_maxLife / 2) - fullHearts;

            for (int i = 0; i < fullHearts + emptyHearts; i++) {

                if (i < fullHearts) {
                    _fullHearts[i].SetActive(true);
                    _demiHearts[i].SetActive(false);
                    _emptyHearts[i].SetActive(false);
                }

                else {
                    _fullHearts[i].SetActive(false);
                    _demiHearts[i].SetActive(false);
                    _emptyHearts[i].SetActive(true);
                }
            }
        }
    }
    #endregion


}
