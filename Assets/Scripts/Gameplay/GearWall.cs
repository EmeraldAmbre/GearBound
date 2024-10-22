using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCompositePhysics>().m_isOnContactWithGearWall = true;
            // Player is left the gear wall
            if (transform.position.x > collision.transform.position.x) collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = 1;
            // Player is right the gear wall
            else collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = -1;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCompositePhysics>().m_isOnContactWithGearWall = false;
            collision.gameObject.GetComponent<PlayerCompositePhysics>().m_gearWallDirection = 0;

        }
    }
}
