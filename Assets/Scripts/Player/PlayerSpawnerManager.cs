using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerManager : MonoBehaviour
{
    [SerializeField] bool _isSpawingPlayerOnRoomConnector = false;
    private void Awake()
    {
        if(_isSpawingPlayerOnRoomConnector)
        {

            PlayerController player = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;
            
            // Get the room connector where to spawn player
            RoomConnector[] arrayRoomConnector = FindObjectsOfType(typeof(RoomConnector)) as RoomConnector[];
            List<RoomConnector> listRoomConnectors = new List<RoomConnector>();
            listRoomConnectors.AddRange(arrayRoomConnector);
            RoomConnector roomConnectorToSpawnPlayer = listRoomConnectors.Find(
                roomCoonnector => roomCoonnector.m_SceneNameToConnect == RoomData.Instance.m_lastRoomSceneName);

            player.transform.position = roomConnectorToSpawnPlayer.m_PointToSpawnPlayer.transform.position;
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
