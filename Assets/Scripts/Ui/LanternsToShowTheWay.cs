using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternsToShowTheWay : MonoBehaviour {

    [SerializeField] GameObject _lantern1;
    [SerializeField] GameObject _lantern2;
    [SerializeField] GameObject _lantern3;
    [SerializeField] GameObject _lantern4;

    void Update() {

        int index = 0;

        if (PlayerPrefs.GetInt("dash") == 1) { index += 1; }
        if (PlayerPrefs.GetInt("magnet") == 1) { index += 1; }
        if (PlayerPrefs.GetInt("rotation") == 1) { index += 1; }
        if (PlayerPrefs.GetInt("possession") == 1) { index += 1; }

        switch (index) {
            case 0:
                _lantern1.SetActive(true);
                break;
            case 1:
                _lantern2.SetActive(true);
                break;
            case 2:
                _lantern3.SetActive(true);
                break;
            case 3:
                _lantern4.SetActive(true);
                break;
            case 4:
                _lantern1.SetActive(false);
                _lantern2.SetActive(false);
                _lantern3.SetActive(false);
                _lantern4.SetActive(false);
                break;
        }
    }
}
