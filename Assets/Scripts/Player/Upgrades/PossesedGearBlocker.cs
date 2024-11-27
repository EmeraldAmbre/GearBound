using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossesedGearBlocker : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] BoxCollider2D _collider;
    void Start()
    {
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerUpgrades>().m_isPossessed )
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }
}
