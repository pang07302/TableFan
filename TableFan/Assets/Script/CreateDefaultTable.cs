using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.IO;


public class CreateDefaultTable : MonoBehaviour
{
    [SerializeField] private string endpoint = "http://localhost:8000";
    public Service service;

    void Start()
    {
        string deviceTable = System.IO.File.ReadAllText("Assets/Json/DeviceTable.json");
        StartCoroutine(service.SendReq($"{endpoint}/createDeviceTable", service.ToByteArray(deviceTable)));
        string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
        StartCoroutine(service.SendReq($"{endpoint}/createEffectTable", service.ToByteArray(effectTable)));
    }
}
