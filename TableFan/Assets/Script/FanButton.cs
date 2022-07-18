using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.IO;

public class FanButton : MonoBehaviour
{
    public BtnOff btnOff;
    public float speed;
    public Blade blade;
    public AudioSource audiosource;
    public AudioClip push;
    public Panel panel;
    public CreateDevice service = new CreateDevice();

    // public AudioClip bounce;
    // [SerializeField] private string authericationEndpoint = "http://localhost:8000/fan";
    // void Start()
    // {
    //     GenerateDefaultTable();
    // }
    public void GenerateDefaultTable()
    {
        string deviceTable = System.IO.File.ReadAllText("Assets/Json/DeviceTable.json");
        StartCoroutine(service.SendReq("http://localhost:8000/createDeviceTable", service.ToByteArray(deviceTable)));
        string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
        StartCoroutine(service.SendReq("http://localhost:8000/createEffectTable", service.ToByteArray(effectTable)));
        // string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
    }

    private void OnMouseDown()
    {
        GenerateDefaultTable();
        btnOff.BounceAll();
        transform.DOLocalMoveY(-0.3f, 0.1f);
        audiosource.PlayOneShot(push);
        blade.SetSpeed(speed);
        Debug.Log(speed);

        string fan = JsonUtility.ToJson(new Devices(1, "On"));

        StartCoroutine(service.SendReq("http://localhost:8000/fans", service.ToByteArray(fan)));
        string id = "fan01"; // device id
        string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");

        // StartCoroutine(SendReq("http://localhost:8000/haptic/" + id, ToByteArray(haptic_effects)));

        panel.CreateDefaultEffect("Haptic", id, speed, null);

        // StartCoroutine(SendReq("http://192.168.1.14:8000/fans", fan.generateDevice(1)));

        // StartCoroutine(SendReq("http://192.168.1.14:8000/fan/", "On"));


    }





    public void Bounce()
    {
        transform.DOLocalMoveY(0f, 0.1f);
    }
}
