using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScenesControl : MonoBehaviour
{
    public GameObject selectPack;
    public NetworkManagerHUD hud;
    public string packName;

    void Start()
    {
#if UNITY_IOS || UNITY_ANDROID
        string loadPath = Application.persistentDataPath;
#else
        string loadPath = Application.dataPath;
#endif
        hud = gameObject.GetComponent<NetworkManagerHUD>();
        string []pathList = Directory.GetFiles(loadPath + "/AssetBundles/");
        List<string> optionList = new List<string>();
        foreach(var item in pathList)
        {
            if(Path.GetExtension(item) == ".art")
            {
                optionList.Add(Path.GetFileNameWithoutExtension(item));
            }
        }
        selectPack.GetComponent<Dropdown>().AddOptions(optionList);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "WaitHoust")
        {
            hud.offsetX = Screen.width / 2 - 100; ;
            hud.offsetY = Screen.height - 200;
            packName = selectPack.transform.Find("Label").GetComponent<Text>().text;
        }
        else
        {
            hud.offsetX = Screen.width - 215; ;
            hud.offsetY = -50;
        }
    }
}
