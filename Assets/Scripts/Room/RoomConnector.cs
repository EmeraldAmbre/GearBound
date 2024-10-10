using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                List<MechanismData> listMechanismDataToSave = new List<MechanismData>();
                for (int i = 0; i < listMechanismToSaveState.Count; i++)
                {
                    MechanismData mechanismData = new MechanismData(listMechanismToSaveState.ElementAt(i));
                    listMechanismDataToSave.Add(mechanismData);
                }
                roomMechanismData = new RoomMechanismData();
                roomMechanismData.m_SceneRoomName = SceneManager.GetActiveScene().name;
                roomMechanismData.m_ListMechanismStateSaved = listMechanismDataToSave;
                RoomData.Instance.m_ListRoomMechanismData.Add(roomMechanismData);

            
            }
        }
    }
}
