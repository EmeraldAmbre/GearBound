using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomConnector : MonoBehaviour {

    public string m_SceneNameToConnect;
    public Transform m_PointToSpawnPlayer;

    GameObject _player;
    PlayerManager _manager;

    void Awake() {

        _player = GameObject.FindWithTag("Player");
        _manager = _player.GetComponent<PlayerManager>();

    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == 7)
        {
            StartCoroutine(MakeRoomTransition());
        }
    }



    IEnumerator MakeRoomTransition()
    {
        AudioManager.Instance.StopMusic();
        SaveRoomData();
        _manager.ChangeRoom();
        yield return new WaitForSeconds(0.5f);
    }

    private void SaveRoomData()
    {
        SceneManager.LoadScene(m_SceneNameToConnect);
        RoomData.Instance.m_LastRoomSceneName = SceneManager.GetActiveScene().name;

        // If not roomMechanism Data saved yet for this room
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
                if (mechanismData.name == "---- Tilemap  to rotate")
                {
                    Debug.Log("FIRST TIME SAVING SAVING THE tilemaptorotate : for other time");
                    Debug.Log("mechanismToChange.transform.rotation : " + mechanismData.rotation);
                    Debug.Log("mechanismStateSaved.name : " + mechanismData.name);
                }
            }
            roomMechanismData = new RoomMechanismData();
            roomMechanismData.m_SceneRoomName = SceneManager.GetActiveScene().name;
            roomMechanismData.m_ListMechanismStateSaved = listMechanismDataToSave;
            RoomData.Instance.m_ListRoomMechanismData.Add(roomMechanismData);
        }
        else
        {
            RoomMechanismData data = RoomData.Instance.m_ListRoomMechanismData.Find(data => data.m_SceneRoomName == SceneManager.GetActiveScene().name);
            data.m_ListMechanismStateSaved.Clear();

            List<GameObject> listMechanismToSaveState = new List<GameObject>();
            listMechanismToSaveState.AddRange(GameObject.FindGameObjectsWithTag("MechanismToSaveState"));

            for (int i = 0; i < listMechanismToSaveState.Count; i++)
            {
                MechanismData mechanismData = new MechanismData(listMechanismToSaveState.ElementAt(i));
                data.m_ListMechanismStateSaved.Add(mechanismData);
                if (mechanismData.name == "---- Tilemap  to rotate")
                {
                    Debug.Log("SAVING THE tilemaptorotate : ");
                    Debug.Log("mechanismToChange.transform.rotation : " + mechanismData.rotation);
                    Debug.Log("mechanismStateSaved.name : " + mechanismData.name);
                }
            }
        }
    }
}
