using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClientMaster : NetworkBehaviour
{

    ServerMaster GM;

    ObjectMap cube;
    string cubeName;
    public GameObject CreateObject;

    public GameObject okButton;
    public GameObject addButton;
    public GameObject plant;
    int controlMode = 0;
    public Camera cameras;

    public Dictionary< string, ObjectMap > OM = new Dictionary<string, ObjectMap>();
    public struct ObjectMap
    {
        public GameObject ob;
        public float movePotionX;
        public float movePotionY;
        public float movePotionZ;
    }

	// Use this for initialization
	void Start ()
    {
        if (cameras == null)
            cameras = Camera.main;
        if (isServer)
        {
            GM = GameObject.Find("ServerMaster").GetComponent<ServerMaster>();
            GM.Login(this);
        }
        if (isLocalPlayer)
        {
            okButton = GameObject.Find("OKButton");
            okButton.GetComponent<Button>().onClick.AddListener(CheckOK);
            CmdGetAllObject();
        }
    }
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && isLocalPlayer)
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && isLocalPlayer)
#endif
        {
            if (cube.ob != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << (LayerMask.NameToLayer("noRay"));
                layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    Vector3 path = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    float[] lenX = new float[] { Mathf.Abs(hit.transform.position.x - path.x), Mathf.Abs(hit.transform.position.y - path.y), Mathf.Abs(hit.transform.position.z - path.z) };
                    
                    Debug.Log(hit.transform.name + ":" + hit.transform.position);
                    Debug.Log("point:" + hit.point);
                    Debug.Log("len:" + lenX[0] + " " + lenX[1] + " " + lenX[2]);
                    if (Math.Round(lenX[0], 3) >= Math.Round(hit.collider.bounds.size.x / 2, 3))
                    {
                        Debug.Log("X");
                        if (hit.transform.position.x - path.x > 0)
                            path.x -= cube.movePotionX * GameObject.Find("basePut").transform.lossyScale.x;
                        else
                            path.x += cube.movePotionX * GameObject.Find("basePut").transform.lossyScale.x;
                    }
                    else if (Math.Round(lenX[1], 3) >= Math.Round(hit.collider.bounds.size.y / 2, 3))
                    {
                        Debug.Log("Y");
                        if (hit.transform.position.y - path.y > 0)
                            path.y -= cube.movePotionY * GameObject.Find("basePut").transform.lossyScale.y;
                        else
                            path.y += cube.movePotionY * GameObject.Find("basePut").transform.lossyScale.y;
                    }
                    else if (Math.Round(lenX[2], 3) >= Math.Round(hit.collider.bounds.size.z / 2, 3))
                    {
                        Debug.Log("Z");
                        if (hit.transform.position.z - path.z > 0)
                            path.z -= cube.movePotionZ * GameObject.Find("basePut").transform.lossyScale.z;
                        else
                            path.z += cube.movePotionZ * GameObject.Find("basePut").transform.lossyScale.z;
                    }

                    cube.ob.gameObject.transform.position = path;
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << (LayerMask.NameToLayer("noRay"));
                layerMask = ~layerMask;
                //layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    string[] objectName = hit.transform.gameObject.name.Split('_');
                    if (objectName.Length == 2)
                    {
                        Debug.Log(this.name);
                        Debug.Log(controlMode);
                        controlMode = 2;
                        Debug.Log(controlMode);
                        cube.ob = hit.transform.gameObject;
                        cubeName = objectName[0];
                        cube.movePotionX = OM[cubeName].movePotionX;
                        cube.movePotionY = OM[cubeName].movePotionY;
                        cube.movePotionZ = OM[cubeName].movePotionZ;
                        cube.ob.gameObject.layer = LayerMask.NameToLayer("noRay");
                        cube.ob.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Outline Shader");
                    }
                }
            }
        }
    }

    void AddTempObject(string typeName)
    {
        if (cube.ob != null)
            CheckOK();
        cube.ob = Instantiate(OM[typeName].ob, GameObject.Find("basePut").transform);
        cubeName = typeName;
        cube.movePotionX = OM[typeName].movePotionX;
        cube.movePotionY = OM[typeName].movePotionY;
        cube.movePotionZ = OM[typeName].movePotionZ;
        cube.ob.transform.position = new Vector3(0, 0.5f, 0);
        cube.ob.name = cube.ob.name.Replace("(Clone)", "");
        cube.ob.gameObject.layer = LayerMask.NameToLayer("noRay");
        cube.ob.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Outline Shader");
        controlMode = 1;
    }

    void CheckOK()
    {
        Debug.Log(this.name);
        Debug.Log(controlMode);
        if (controlMode == 1)
        {
            CmdAddObjectInList(cubeName, ObjectStatue.ADD, cube.ob.transform.position, cube.ob.transform.rotation);
            Destroy(cube.ob);
            cube.ob = null;
            cubeName = "";
        }
        else if (controlMode == 2)
        {
            Debug.Log("Fix");
            CmdFixObjectPath(cube.ob.name, cube.ob.transform.position);
            cube.ob.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
            cube.ob.gameObject.layer = 0;
            cube.ob = null;
            cubeName = "";
        }
        controlMode = 0;
    }

    [Command]
    public void CmdGetAllObject()
    {
        GM.GetAllObject(this);
    }

    [Command]
    public void CmdAddObjectInList(string name, ObjectStatue status, Vector3 position, Quaternion rotation)
    {
        GM.AddObjectInList(name, status, position, rotation);
    }

    [Command]
    public void CmdFixObjectPath(string name, Vector3 position)
    {
        string[] objectName = name.Split('_');
        GM.FixObjectPath(Int32.Parse(objectName[1]), position);
    }

    [ClientRpc]
    public void RpcSetPlayer(int id)
    {
        this.name = "player" + id.ToString();
        Debug.Log("Login");
    }

    [ClientRpc]
    public void RpcAddObjectInList(string type, string name, Vector3 position, Quaternion rotation)
    {
        if (isLocalPlayer)
        {
            GameObject addIt = Instantiate(OM[type].ob, position, rotation, GameObject.Find("basePut").transform);
            addIt.name = type + "_" + name;
        }
    }

    [ClientRpc]
    public void RpcFixObjectPath(string type, string name, Vector3 position)
    {
        if (isLocalPlayer)
        {
            Debug.Log("Fix");
            GameObject.Find(type + "_" + name).transform.position = position;
        }
    }

    [ClientRpc]
    public void RpcReadAbb(string bundleName)
    {
        if (isLocalPlayer)
        {
#if UNITY_IOS || UNITY_ANDROID
            string loadPath = Application.persistentDataPath;
#else
            string loadPath = Application.dataPath;
#endif
            var bu = AssetBundle.LoadFromFile(loadPath + "/AssetBundles/" + bundleName);
            string json = bu.LoadAsset("index").ToString();
            var loadData = JsonUtility.FromJson<JsonRead>(json);
            foreach (var item in loadData.objct)
            {
                if (item.type == "object")
                {
                    ObjectMap tmp = new ObjectMap();
                    tmp.ob = bu.LoadAsset(item.Name) as GameObject;
                    tmp.movePotionX = item.MovePotion[0];
                    tmp.movePotionY = item.MovePotion[1];
                    tmp.movePotionZ = item.MovePotion[2];
                    OM.Add(item.Name, tmp);
                }
            }
            plant = GameObject.Find("Panel");
            int i = 0;
            foreach (var item in loadData.playerHas)
            {
                GameObject tmp = Instantiate(addButton, plant.transform);
                tmp.GetComponent<Button>().onClick.AddListener(() => AddTempObject(item));
                tmp.transform.Find("Text").GetComponent<Text>().text = item;
                tmp.GetComponent<RectTransform>().localPosition = new Vector2(50 + 100 * i , 0);
                i++;
            }
        }
    }
}
