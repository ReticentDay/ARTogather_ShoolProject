using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ObjectInfo
{
    public string objectType;
    public ObjectStatue status;
    public Vector3 position;
    public Quaternion rotation;
};

public class ServerMaster: MonoBehaviour
{
    public List<ClientMaster> allPlayer = new List<ClientMaster>();
    public List<ObjectInfo> objectList = new List<ObjectInfo>();

    public void Login(ClientMaster player)
    {
        allPlayer.Add(player);
        player.RpcSetPlayer(allPlayer.Count - 1);
    }

    public void GetAllObject(ClientMaster player)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            player.RpcAddObjectInList(objectList[i].objectType, i.ToString(), objectList[i].position, objectList[i].rotation);
        }
    }

    public void AddObjectInList(string type, ObjectStatue status, Vector3 position, Quaternion rotation)
    {
        ObjectInfo temp;
        temp.objectType = type;
        temp.status = status;
        temp.position = position;
        temp.rotation = rotation;
        objectList.Add(temp);
        foreach (ClientMaster player in allPlayer)
        {
            player.RpcAddObjectInList(type, (objectList.Count-1).ToString(), position, rotation);
        }
    }

    public void FixObjectPath(int id, Vector3 position)
    {
        ObjectInfo temp = objectList[id];
        temp.position = position;
        objectList[id] = temp;
        foreach (ClientMaster player in allPlayer)
        {
            player.RpcFixObjectPath(temp.objectType, id.ToString(), position);
        }
    }
}