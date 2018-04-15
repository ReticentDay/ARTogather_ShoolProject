using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class rayOnhit : MonoBehaviour {

    public GameObject mainControler;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == "point")
                    {
                        Debug.Log(hit.transform.position);
                        mainControler.GetComponent<MainControler>().HitOnTrue = false;
                        mainControler.GetComponent<MainControler>().AddClub(hit.transform.gameObject);
                    }
                }
            }
        }
    }
}
