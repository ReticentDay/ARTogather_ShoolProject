using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using client;

public class MainControler : MonoBehaviour {
    public bool HitOnTrue;
    public static Client CC;
    public GameObject CreateObject;
    public List<ObjectListStruct> ObjectList;
    public int updateCount = 5;
    public class ObjectListStruct
    {
        public int Id;
        public int flag;
        public GameObject GObject;
        public float x, y, z;
    };

    // Use this for initialization
    void Start () {
        HitOnTrue = false;
        CC._CAC += new Client.CallAndCatch(readDate);
        ObjectList = new List<ObjectListStruct>();
        MainControler.CC.WriteData("Need:GGININ");
    }

    // Update is called once per frame
    void Update()
    {
        updateCount--;
        if (updateCount < 0)
        {
            for (int i = 0; i < ObjectList.Count; i++)
            {
                if (ObjectList[i].flag == 1)
                {
                    ObjectList[i].GObject = AddObjectPosition(ObjectList[i].x, ObjectList[i].y, ObjectList[i].z);
                    ObjectList[i].flag = 0;
                }
            }
            updateCount = 5;
        }
    }

    public void ReadyToAdd()
    {
        Debug.Log("Button");
        HitOnTrue = true;
    }

    public void AddClub(GameObject point)
    {
        point.GetComponent<PointScript>().SendCreatMessage();
    }

    void OnApplicationQuit()
    {
        CC.StopClient();
    }

    public void readDate(string message)
    {
        Debug.Log("NET: " + message);
        string[] messages = message.Split(':');
        if (messages[0] == "add")
        {
            if (messages[2] == "position")
            {
                ObjectListStruct OLS = new ObjectListStruct();
                OLS.flag = 1;
                OLS.Id = int.Parse(messages[1]);
                OLS.x = float.Parse(messages[3]);
                OLS.y = float.Parse(messages[4]);
                OLS.z = float.Parse(messages[5]);
                ObjectList.Add(OLS);
            }
        }
    }

    GameObject AddObjectPosition(float x,float y,float z)
    {
        GameObject beCreateObject = Instantiate(CreateObject);
        beCreateObject.transform.position = new Vector3(x, y, z);
        beCreateObject.name = beCreateObject.name.Replace("(Clone)", "");
        return beCreateObject;
    }
}
