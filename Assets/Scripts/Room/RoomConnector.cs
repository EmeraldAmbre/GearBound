using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomConnector : MonoBehaviour
{
    public string m_SceneNameToConnect;
    public Transform m_PointToSpawnPlayer;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("last scene name from playerSpawnerManager :" + RoomData.Instance.m_LastRoomSceneName);
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
            RoomData.Instance.m_LastRoomSceneName = SceneManager.GetActiveScene().name;

            // If not roomMechanism Data saved yert for this room
            RoomMechanismData roomMechanismData = RoomData.Instance.m_ListRoomMechanismData.Find(
                roomMechanicData => roomMechanicData.m_SceneRoomName == SceneManager.GetActiveScene().name);

            if (roomMechanismData == null)
            {
                List<GameObject> listMechanismToSaveState = new List<GameObject>();
                listMechanismToSaveState.AddRange(GameObject.FindGameObjectsWithTag("MechanismToSaveState"));
                roomMechanismData = new RoomMechanismData();
                roomMechanismData.m_SceneRoomName = SceneManager.GetActiveScene().name;
                roomMechanismData.m_ListMechanismStateSaved = listMechanismToSaveState;
                RoomData.Instance.m_ListRoomMechanismData.Add(roomMechanismData); ;

            
            }
        }
    }
}
