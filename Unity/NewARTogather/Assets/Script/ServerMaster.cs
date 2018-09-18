using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public struct ObjectInfo
{
    public string objectType;
    public ObjectStatue status;
    public Vector3 position;
    public Quaternion rotation;
};

public struct JsonRead
{
    [Serializable]
    public struct objectSS
    {
        public string Name;
        public string type;
        public List<float> MovePotion;
    }
    [Serializable]
    public struct baseInfo
    {
        public string Name;
        public List<float> Position;
        public List<float> Rosition;
    }
    public List<objectSS> objct;
    public List<baseInfo> basePlant;
    public List<string> playerHas;
}

public class ServerMaster: MonoBehaviour
{
    public List<ClientMaster> allPlayer = new List<ClientMaster>();
    public List<ObjectInfo> objectList = new List<ObjectInfo>();
    public string openPack;

    private void Start()
    {
#if UNITY_IOS || UNITY_ANDROID
        string loadPath = Application.persistentDataPath;
#else
        string loadPath = Application.dataPath;
#endif
        Debug.Log(GameObject.Find("NetworkMng").GetComponent<ScenesControl>().packName);
        openPack = GameObject.Find("NetworkMng").GetComponent<ScenesControl>().packName + ".art";
        var bu = AssetBundle.LoadFromFile(loadPath + "/AssetBundles/" + openPack);
        string json = bu.LoadAsset("index").ToString();
        var loadData = JsonUtility.FromJson<JsonRead>(json);
        foreach (var item in loadData.basePlant)
        {
            ObjectInfo tmp = new ObjectInfo();
            tmp.objectType = item.Name;
            tmp.position = new Vector3(item.Position[0], item.Position[1], item.Position[2]);
            tmp.rotation = new Quaternion(item.Rosition[0], item.Rosition[1], item.Rosition[2], item.Rosition[3]);
            tmp.status = ObjectStatue.NONE;
            objectList.Add(tmp);
        }
        bu.Unload(false);
    }

    public void Login(ClientMaster player)
    {
        allPlayer.Add(player);
        player.RpcSetPlayer(allPlayer.Count - 1);
    }

    public void GetAllObject(ClientMaster player)
    {
        player.RpcReadAbb(openPack);
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