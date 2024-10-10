using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomData : Singleton<RoomData>
{
    public string m_LastRoomSceneName;

    public List<RoomMechanismData> m_ListRoomMechanismData = new List<RoomMechanismData>();

}

public class RoomMechanismData
{
    public string m_SceneRoomName;
    public List<MechanismData> m_ListMechanismStateSaved = new List<MechanismData>();
    
}

public class MechanismData
{
    public string name;
    public Vector3 position;

    public MechanismData(GameObject gameObject)
    {
        name = gameObject.name;
        position = gameObject.transform.position;
    }
}