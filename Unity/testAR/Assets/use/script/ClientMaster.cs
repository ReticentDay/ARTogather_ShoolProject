using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ObjectStatue
{
    NONE = 0,
    ADD
}

public class ClientMaster : MonoBehaviour {

    ObjectInfo cube;
    public float[] movePotion = new float[3];
    public GameObject CreateObject;
    public struct ObjectInfo {
        public GameObject ob;
        public ObjectStatue status;
    };
    public List<ObjectInfo> objectList;

    void Start () {
        objectList = new List<ObjectInfo>();
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
                if (lenX[0] >= lenX[1] && lenX[0] >= lenX[2]){
                    if (hit.transform.position.x - path.x > 0)
                        path.x -= movePotion[0];
                    else
                        path.x += movePotion[0];
                }
                else if (lenX[1] >= lenX[0] && lenX[1] >= lenX[2])
                {
                    if (hit.transform.position.y - path.y > 0)
                        path.y -= movePotion[1];
                    else
                        path.y += movePotion[1];
                }
                else if (lenX[2] >= lenX[1] && lenX[2] >= lenX[0])
                {
                    if (hit.transform.position.z - path.z > 0)
                        path.z -= movePotion[2];
                    else
                        path.z += movePotion[2];
                }

                cube.ob.transform.position = path;
            }
        }
    }

    public void addButton()
    {
        if (cube.ob != null)
            checkOK();
        cube.ob = Instantiate(CreateObject);
        cube.ob.transform.position = new Vector3(0, 0.5f, 0);
        cube.ob.name = cube.ob.name.Replace("(Clone)", "");
        cube.status = ObjectStatue.ADD;
        cube.ob.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Outline Shader");
    }

    public void checkOK()
    {
        cube.ob.gameObject.layer = 0;
        cube.ob.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
        objectList.Add(cube);
        cube.ob = null;
        cube.status = ObjectStatue.NONE;
    }
}
