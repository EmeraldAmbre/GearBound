using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearWall : MonoBehaviour
{
    static int _nbgearWallInContact = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_nbgearWallInContact == 0) collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<PlayerCompositePhysics>().m_isOnContactWithGearWall = true;
            _nbgearWallInContact++;
            // Player is left the gear wall
            if (transform.position.x > collision.transform.position.x) collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = 1;
            // Player is right the gear wall
            else collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = -1;
            collision.gameObject.GetComponent<PlayerController>().m_isGearWallJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _nbgearWallInContact--;
            if(_nbgearWallInContact == 0)
            {
                collision.gameObject.GetComponent<PlayerCompositePhysics>().m_isOnContactWithGearWall = false;
                collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = 0;
            }

        }
    }
}
