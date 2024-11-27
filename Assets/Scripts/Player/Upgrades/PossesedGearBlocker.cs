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
        if (!collision.gameObject.GetComponent<PlayerUpgrades>().m_isPossessed )
        {
            _collider.enabled = true;
            collision.gameObject.GetComponent<PlayerUpgrades>().m_gearToControl = null;

            GameObject gearToControl = collision.gameObject.GetComponent<PlayerUpgrades>().m_gearToControl;

            ParticleSystem.EmissionModule emission = gearToControl.GetComponentInChildren<ParticleSystem>().emission;
            if (gearToControl.GetComponentInChildren<ParticleSystem>().isPlaying) gearToControl.GetComponentInChildren<ParticleSystem>().Stop();
            emission.enabled = false;
        }
        else
        {
            _collider.enabled = false;
        }
    }
}
