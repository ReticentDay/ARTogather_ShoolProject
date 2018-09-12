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
    int controlMode = 0;

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
        ObjectMap temp;
        temp.ob = CreateObject;
        temp.movePotionX = temp.movePotionY = temp.movePotionZ = 0.5f;
        OM.Add("cube", temp);
        if (isServer)
        {
            GM = GameObject.Find("ServerMaster").GetComponent<ServerMaster>();
            GM.Login(this);
        }
        if (isLocalPlayer)
        {
            okButton = GameObject.Find("OKButton");
            addButton = GameObject.Find("AddObject");
            addButton.GetComponent<Button>().onClick.AddListener(AddTempObject);
            okButton.GetComponent<Button>().onClick.AddListener(CheckOK);
            CmdGetAllObject();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && isLocalPlayer)
        {
            if (cube.ob != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << (LayerMask.NameToLayer("noRay"));
                layerMask = ~layerMask;
                //layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, 30.0f, layerMask))
                {
                    //Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
                    //Debug.Log(hit.transform.position);
                    //Debug.Log("x:" + hit.point.x + " y:" + hit.point.y + " z:" + hit.point.z);
                    Vector3 path = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    float[] lenX = new float[] { Mathf.Abs(hit.transform.position.x - path.x), Mathf.Abs(hit.transform.position.y - path.y), Mathf.Abs(hit.transform.position.z - path.z) };
                    if (lenX[0] >= lenX[1] && lenX[0] >= lenX[2])
                    {
                        if (hit.transform.position.x - path.x > 0)
                            path.x -= cube.movePotionX;
                        else
                            path.x += cube.movePotionX;
                    }
                    else if (lenX[1] >= lenX[0] && lenX[1] >= lenX[2])
                    {
                        if (hit.transform.position.y - path.y > 0)
                            path.y -= cube.movePotionY;
                        else
                            path.y += cube.movePotionY;
                    }
                    else if (lenX[2] >= lenX[1] && lenX[2] >= lenX[0])
                    {
                        if (hit.transform.position.z - path.z > 0)
                            path.z -= cube.movePotionZ;
                        else
                            path.z += cube.movePotionZ;
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
                if (Physics.Raycast(ray, out hit, 30.0f, layerMask))
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

    void AddTempObject()
    {
        if (cube.ob != null)
            CheckOK();
        cube.ob = Instantiate(OM["cube"].ob);
        cubeName = "cube";
        cube.movePotionX = OM["cube"].movePotionX;
        cube.movePotionY = OM["cube"].movePotionY;
        cube.movePotionZ = OM["cube"].movePotionZ;
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
            GameObject addIt = Instantiate(OM[type].ob, position, rotation);
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
}
