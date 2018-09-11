using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using client;
using UnityEngine.SceneManagement;


public class TryToLink : MonoBehaviour {
    public Text Log;
	// Use this for initialization
	void Start () {
        MainControler.CC = new Client("192.168.1.110", 36000);
        try
        {
            MainControler.CC.StartClient();
            SceneManager.LoadScene("BuildingBlock");

        }
        catch (NetException e)
        {
            Log.text += e.Message;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
