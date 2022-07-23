using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CustomisedDevice : MonoBehaviour
{
    [SerializeField] private TMP_InputField TMPname;
    [SerializeField] private TMP_Dropdown TMPcategory;
    [SerializeField] private string endpoint = "http://192.168.1.14:8000";


    public CreateDevice service = new CreateDevice();
    private static string name = "";
    static string category;

    // void Update()
    // {
    //     name = TMPname.text;
    //     category = TMPcategory.options[TMPcategory.value].text;
    // }

    public void ManualAddDevice()
    {
        name = TMPname.text;
        category = TMPcategory.options[TMPcategory.value].text;

        string customDevice = JsonUtility.ToJson(new Devices(name, category, null));
        Debug.Log(customDevice);

        StartCoroutine(service.SendReq($"{endpoint}/customDevice", service.ToByteArray(customDevice)));

    }
}
