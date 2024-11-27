using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToActivateText : MonoBehaviour {

    [SerializeField] GameObject _txtToActivate;

    private void OnTriggerEnter2D(Collider2D collision) {
        _txtToActivate.SetActive(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _txtToActivate.SetActive(false);
    }
}
