using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnerManager : MonoBehaviour
{
    [SerializeField] bool _isSpawingPlayerOnRoomConnector = false;
    private void Awake()
    {
        if (_isSpawingPlayerOnRoomConnector)
        {
            PlayerController player = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;

            // Get the room connector where to spawn player
            RoomConnector[] arrayRoomConnector = FindObjectsOfType(typeof(RoomConnector)) as RoomConnector[];
            List<RoomConnector> listRoomConnectors = new List<RoomConnector>();
            listRoomConnectors.AddRange(arrayRoomConnector);
            RoomConnector roomConnectorToSpawnPlayer = listRoomConnectors.Find(
                roomCoonnector => roomCoonnector.m_SceneNameToConnect == RoomData.Instance.m_LastRoomSceneName);

            player.transform.position = roomConnectorToSpawnPlayer.m_PointToSpawnPlayer.transform.position;
        }


        RoomMechanismData roomMechanismData = RoomData.Instance.m_ListRoomMechanismData.Find(
                roomMechanicData => roomMechanicData.m_SceneRoomName == SceneManager.GetActiveScene().name);

        if (roomMechanismData != null)
        {
            List<GameObject> listMechanismToChangeState = new List<GameObject>();
            listMechanismToChangeState.AddRange(GameObject.FindGameObjectsWithTag("MechanismToSaveState"));



            for (int i = 0; i < listMechanismToChangeState.Count; i++)
            {
                GameObject mechanismToChange = listMechanismToChangeState.ElementAt(i);
                for (int j = 0; j < roomMechanismData.m_ListMechanismStateSaved.Count; j++)
                {
                    GameObject mechanismStateSaved = roomMechanismData.m_ListMechanismStateSaved.ElementAt(i);
                    if (mechanismToChange.name == mechanismStateSaved.name)
                    {
                        mechanismToChange = mechanismStateSaved;
                        break;
                    }
                }
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
