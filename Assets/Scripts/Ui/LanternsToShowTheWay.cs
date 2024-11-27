using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternsToShowTheWay : MonoBehaviour
{
    [SerializeField] GameObject _lantern1;
    [SerializeField] GameObject _lantern2;
    [SerializeField] GameObject _lantern3;

    void Update()
    {
        if (PlayerPrefs.GetInt("dash") == 1 && _lantern1.activeSelf == false)
        {
            _lantern1.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("dash") == 0 && _lantern1.activeSelf == true) _lantern1.SetActive(false);

        if (PlayerPrefs.GetInt("magnet") == 1 && _lantern2.activeSelf == false)
        {
            _lantern2.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("magnet") == 0 && _lantern2.activeSelf == true) _lantern2.SetActive(false);

        if (PlayerPrefs.GetInt("rotation") == 1 && _lantern3.activeSelf == false)
        {
            _lantern3.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("rotation") == 0 && _lantern3.activeSelf == true) _lantern3.SetActive(false);
    }
}
