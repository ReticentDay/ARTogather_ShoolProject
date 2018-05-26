using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour {
    public GameObject mainControler;
    MainControler script;
    public float createPositionX, createPositionY, createPositionZ;
    public GameObject CreateObject;

    void Start () {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        mainControler = GameObject.Find("MainControler");
        script = mainControler.GetComponent<MainControler>();
    }
	
	void Update () {
        if (script.HitOnTrue)
            gameObject.layer = LayerMask.NameToLayer("point");
        else
            gameObject.layer = LayerMask.NameToLayer("hidePoint");
    }

    public void Create()
    {
        GameObject beCreateObject = Instantiate(CreateObject);
        Vector3 nowPosition = gameObject.transform.position;
        beCreateObject.transform.position = new Vector3(nowPosition.x + createPositionX, nowPosition.y + createPositionY, nowPosition.z + createPositionZ);
        beCreateObject.name= beCreateObject.name.Replace("(Clone)", "");
    }
}
