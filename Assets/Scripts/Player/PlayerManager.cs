using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public bool m_isInteracting { get; set; }
    public int m_playerLife { get; private set; }
    public int m_rotationInversion { get; private set; }
    public bool m_isInvincible { get; private set; }

    #endregion

    private void Awake() {
        m_isInteracting = false;
        m_rotationInversion = 1;

        m_playerLife = _maxLife;
        LifeUpdate();
    }

    private void Update()
    {
        LifeUpdate();
    }

    #region Public Methods

    public void RotationInversion() {
        m_rotationInversion *= -1;
    }

    public void TakeDamage(int nb_damage = 1) {
        if (!m_isInvincible) {
            if (m_playerLife > 0) m_playerLife -= nb_damage;
            LifeUpdate();
            StartCoroutine(Invincibility());
        }
    }

    public void Heal() {
        if (m_playerLife < _maxLife) m_playerLife += 1;
        LifeUpdate();
    }

    public void LifeUpgrade() {
        _maxLife += 2;
    }
    #endregion

    #region Private Methods
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
