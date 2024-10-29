using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    static int _id = 1;
    static string _name = "rotation";

    public int ID { get { return _id; } private set { _id = value; } }
    public string Name { get { return _name; } private set { _name = value; } }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerPrefs.SetInt(_name, 1);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
        }
    }
}