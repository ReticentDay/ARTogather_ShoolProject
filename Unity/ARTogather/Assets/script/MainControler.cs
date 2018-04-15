using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControler : MonoBehaviour {
    public bool HitOnTrue;
	// Use this for initialization
	void Start () {
        HitOnTrue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadyToAdd()
    {
        Debug.Log("Button");
        HitOnTrue = true;
    }

    public void AddClub(GameObject point)
    {
        point.GetComponent<PointScript>().Create();
    }
}
