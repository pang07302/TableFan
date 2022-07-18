using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CustomisedDevice : MonoBehaviour
{
    [SerializeField] private TMP_InputField TMPname;
    [SerializeField] private TMP_Dropdown TMPcategory;


    public CreateDevice service = new CreateDevice();
    static string name;
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

        StartCoroutine(service.SendReq("http://localhost:8000/customDevice", service.ToByteArray(customDevice)));

    }
}
