using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGearPossesionDetector : MonoBehaviour
{

    [SerializeField] PlayerUpgrades _upgrade;
    [SerializeField] float _circleRadisuDetection = 5;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float maxDistance = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, _circleRadisuDetection, _layerMask);

        if (collision)
        {
            if (collision.gameObject.CompareTag("ControllableGear") && _upgrade.m_canControl)
            {
                _upgrade.m_gearToControl = collision.gameObject;
                ParticleSystem.EmissionModule emission = _upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().emission;
                if ( ! _upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().isPlaying) _upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().Play();
                emission.enabled = true;

            }
            if (collision.gameObject.CompareTag("ControllableGear") && _upgrade.m_isPossessed)
            {
                Rigidbody2D rb = _upgrade.m_gearToControl.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }
        else if (_upgrade.m_gearToControl != null)
        {
            if ((Vector2.Distance(transform.position, _upgrade.m_gearToControl.transform.position) > maxDistance))
            {
                Rigidbody2D rb = _upgrade.m_gearToControl.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                ParticleSystem.EmissionModule emission = _upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().emission;
                if (_upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().isPlaying) _upgrade.m_gearToControl.GetComponentInChildren<ParticleSystem>().Stop();
                emission.enabled = false;
                _upgrade.m_gearToControl = null;
            }
        }

    }

    private void OnDrawGizmos()
    {
        GizmoDrawer.DrawCircle(transform.position, _circleRadisuDetection);
    }


}
