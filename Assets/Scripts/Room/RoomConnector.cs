using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomConnector : MonoBehaviour
{
    public string m_SceneNameToConnect;
    public Transform m_PointToSpawnPlayer;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("last scene name from playerSpawnerManager :" + RoomData.Instance.m_lastRoomSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            SceneManager.LoadScene(m_SceneNameToConnect);
            RoomData.Instance.m_lastRoomSceneName = SceneManager.GetActiveScene().name;
        }
    }
}
