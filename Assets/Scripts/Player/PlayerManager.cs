using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    // UI variables
    [SerializeField] private List<GameObject> _fullHearts;
    [SerializeField] private List<GameObject> _demiHearts;
    [SerializeField] private List<GameObject> _emptyHearts;

    // Life and invincibilité frame
    [SerializeField] private int _maxLife;
    [SerializeField] private float _invincibilityTime = 2f;
    [SerializeField] private float _invincibilityFrame = 0.1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    #region Public Variables
    public int m_playerLife;
    public bool m_isInteracting { get; set; }
    public int m_rotationInversion { get; private set; }
    public bool m_isInvincible { get; private set; }

    // Singleton
    public static PlayerManager instance;
    #endregion

    #region Unity API
    void Awake() {
        else { Destroy(gameObject); }

        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        m_isInteracting = false;
        m_rotationInversion = 1;
        m_playerLife = _maxLife;
    }

    void Start() {
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
    }

    void Update() {
        LifeUpdate();
    }
    #endregion

    #region Public Methods
    public void RotationInversion() {
        m_rotationInversion *= -1;
    }

    public void TakeDamage(int nb_damage = 1) {
        if (!m_isInvincible) {
            if (m_playerLife > 0) {
                m_playerLife -= nb_damage;
                LifeUpdate();
                if (m_playerLife <= 0) Death();
                else StartCoroutine(Invincibility());
            }

        }
    }

    public void Heal() {
        if (m_playerLife < _maxLife) m_playerLife += 1;
        LifeUpdate();
    }

    public void LifeUpgrade() {
        _maxLife += 2;
    }

    public void Death() {

        if (!string.IsNullOrEmpty(CheckpointManager.instance.m_lastCheckpointScene)) {

            if (SceneManager.GetActiveScene().name != CheckpointManager.instance.m_lastCheckpointScene) {
                PlayerPrefs.SetInt("is_respawn", 1); PlayerPrefs.Save();
                SceneManager.LoadScene(CheckpointManager.instance.m_lastCheckpointScene);
                StartCoroutine(RespawnAfterLoading());
            }

            else {
                RespawnPlayer(CheckpointManager.instance.m_lastCheckpointPosition);
            }
        }

        else {
            Debug.LogWarning("Aucun checkpoint enregistré. Respawn à la position initiale.");
            // Définir ici une position par défaut si aucun checkpoint n'a été visité
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator RespawnAfterLoading() {

        yield return null; // Do not delete !
        RespawnPlayer(CheckpointManager.instance.m_lastCheckpointPosition);
    }

    private void RespawnPlayer(Vector3 respawnPosition) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            player.transform.position = respawnPosition;
            m_playerLife = _maxLife;
        }

        else {
            Debug.LogError("Impossible de trouver l'objet Player pour respawn.");
        }
    }

    private IEnumerator Invincibility() {

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
            
            emptyHearts = (_maxLife/2) - fullHearts - 1;

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
            
            emptyHearts = (_maxLife / 2) - fullHearts;

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
