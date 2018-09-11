using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ObjectInfo
{
    public string objectName;
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
        player.RpcSetPlayer();
        foreach (var objectItem in objectList)
        {
            player.RpcAddObjectInList(objectItem.objectName, objectItem.position, objectItem.rotation);
        }
    }


    public void AddObjectInList(string name, ObjectStatue status, Vector3 position, Quaternion rotation)
    {
        ObjectInfo temp;
        temp.objectName = name;
        temp.status = status;
        temp.position = position;
        temp.rotation = rotation;
        objectList.Add(temp);
        foreach (ClientMaster player in allPlayer)
        {
            player.RpcAddObjectInList(name, position, rotation);
        }
    }
}