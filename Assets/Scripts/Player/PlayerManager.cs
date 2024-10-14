using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private List<GameObject> _fullHearts;
    [SerializeField] private List<GameObject> _demiHearts;
    [SerializeField] private List<GameObject> _emptyHearts;
    [SerializeField] private int _maxLife;

    #region Public Variables

    public bool m_isInteracting { get; set; }
    public int m_playerLife;
    public int m_rotationInversion { get; private set; }

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

    public void TakeDamage() {
        if (m_playerLife > 0) m_playerLife -= 1;
        LifeUpdate();
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
