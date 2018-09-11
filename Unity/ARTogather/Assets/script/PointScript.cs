using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using client;

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

    public void SendCreatMessage()
    {
        Vector3 nowPosition = gameObject.transform.position;
        MainControler.CC.WriteData("add:position:"+ (nowPosition.x + createPositionX).ToString() + ":" + (nowPosition.y + createPositionY).ToString() + ":" + (nowPosition.z + createPositionZ).ToString());
    }
}
