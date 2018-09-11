using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClientMaster : NetworkBehaviour
{
    ServerMaster GM;

    ObjectMap cube;
    string cubeName;
    public GameObject CreateObject;

    public GameObject okButton;
    public GameObject addButton;

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
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && cube.ob != null && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << (LayerMask.NameToLayer("noRay"));
            layerMask = ~layerMask;
            //layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, 30.0f, layerMask))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
                Debug.Log(hit.transform.position);
                Debug.Log("x:" + hit.point.x + " y:" + hit.point.y + " z:" + hit.point.z);
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
    }

    void CheckOK()
    {
        CmdAddObjectInList(cubeName, ObjectStatue.ADD, cube.ob.transform.position, cube.ob.transform.rotation);
        Destroy(cube.ob);
        cube.ob = null;
        cubeName = "";
    }

    [Command]
    public void CmdAddObjectInList(string name, ObjectStatue status, Vector3 position, Quaternion rotation)
    {
        GM.AddObjectInList(name, status, position, rotation);
    }

    [ClientRpc]
    public void RpcSetPlayer()
    {
        Debug.Log("Login");
    }

    [ClientRpc]
    public void RpcAddObjectInList(string name, Vector3 position, Quaternion rotation)
    {
        AddItObject(name, position, rotation);
    }

    public void AddItObject(string name, Vector3 position, Quaternion rotation)
    {
        Debug.Log(name);
        Debug.Log(OM.Count);
        GameObject addIt = Instantiate(OM[name].ob, position, rotation);
    }
}
