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
    public Devices device = new Devices();
    public Panel panel;

    // public AudioClip bounce;
    // [SerializeField] private string authericationEndpoint = "http://localhost:8000/fan";
    // void Start()
    // {
    //     GenerateDefaultTable();
    // }
    public void GenerateDefaultTable()
    {
        string deviceTable = System.IO.File.ReadAllText("Assets/Json/DeviceTable.json");
        StartCoroutine(SendReq("http://localhost:8000/createDeviceTable", ToByteArray(deviceTable)));
        string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
        StartCoroutine(SendReq("http://localhost:8000/createEffectTable", ToByteArray(effectTable)));
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

        string fan = JsonUtility.ToJson(device.GenerateDevice(1, "On"));

        // StartCoroutine(SendReq("http://localhost:8000/fans", ToByteArray(fan)));
        string id = "fan01"; // device id
        string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");

        // StartCoroutine(SendReq("http://localhost:8000/haptic/" + id, ToByteArray(haptic_effects)));

        panel.CreateDefaultEffect("Haptic", id, speed, null);




        // StartCoroutine(SendReq("http://192.168.1.14:8000/fans", fan.generateDevice(1)));

        // StartCoroutine(SendReq("http://192.168.1.14:8000/fan/", "On"));


    }

    public byte[] ToByteArray(string json)
    {
        return new System.Text.UTF8Encoding().GetBytes(json);
    }


    public IEnumerator SendReq(string address, byte[] req)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("api-version", "0.1");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }



    public void Bounce()
    {
        transform.DOLocalMoveY(0f, 0.1f);
    }
}
