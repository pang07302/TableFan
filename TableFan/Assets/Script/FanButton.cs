using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.IO;

using Debug = UnityEngine.Debug;

public class FanButton : MonoBehaviour
{
    public List<FanButton> fanbtns = new List<FanButton>();
    public float speed;
    public Blade blade;
    public AudioSource audiosource;
    public AudioClip push;
    public AudioClip pushDown;
    public AudioClip bounceUp;
    public Panel panel;
    public CreateDevice service = new CreateDevice();
    static bool initial = true;
    static long clickTime;
    static long beforeRequestTime;



    // public AudioClip bounce;
    // [SerializeField] private string authericationEndpoint = "http://localhost:8000/fan";
    // void Start()
    // {
    //     GenerateDefaultTable();
    // }
    public void BounceAll()
    {
        foreach (var item in fanbtns)
        {
            item.Bounce();
        }
    }
    // void Start()
    // {
    //     ReadTxt();
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
        clickTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        if (initial)
        {
            GenerateDefaultTable();
            initial = false;
        }

        BounceAll();
        transform.DOLocalMoveY(-0.3f, 0.1f);

        blade.SetSpeed(speed);


        string fan = JsonUtility.ToJson(new Devices(1, this.name));
        if (this.name == "btn_off")
        {
            BounceAll();
            audiosource.PlayOneShot(pushDown);
        }
        else
        {
            audiosource.PlayOneShot(push);
        }

        StartCoroutine(SendReq("http://localhost:8000/fans", service.ToByteArray(fan)));
        string id = "fan"; // device id
        string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");

        // StartCoroutine(SendReq("http://localhost:8000/haptic/" + id, ToByteArray(haptic_effects)));


        panel.CreateDefaultEffect("Haptic", id, this.name, speed, null);

        // StartCoroutine(SendReq("http://192.168.1.14:8000/fans", fan.generateDevice(1)));

        // StartCoroutine(SendReq("http://192.168.1.14:8000/fan/", "On"));
    }
    private void OnMouseUp()
    {
        if (this.name == "btn_off") { Bounce(); }
    }
    public void Bounce()
    {
        transform.DOLocalMoveY(0f, 0.1f);
        if (speed == 0.0) { audiosource.PlayOneShot(bounceUp); }
    }



    public IEnumerator SendReq(string address, byte[] req)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("api-version", "0.1");

        beforeRequestTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            string serverExecuteTimeStr = request.downloadHandler.text.Split(',', ':')[3];
            string afterRequestTimeStr = request.downloadHandler.text.Split(',', ':')[5];
            MeasureTime(serverExecuteTimeStr, afterRequestTimeStr, beforeRequestTime);
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public void MeasureTime(string serverExecuteTimeStr, string afterRequestTimeStr, long beforeRequestTime)
    {
        Debug.Log(clickTime + " , " + beforeRequestTime);


        double.TryParse(serverExecuteTimeStr, out double serverExecuteTime);
        long.TryParse(afterRequestTimeStr, out long afterRequestTime);
        double time = (double)(beforeRequestTime - clickTime);
        double requestTime = (double)(afterRequestTime - beforeRequestTime);
        Debug.Log(requestTime);

        string delayTime = (time + serverExecuteTime + afterRequestTime - beforeRequestTime) / 1000 + "";
        WriteTxt(time / 1000, serverExecuteTime / 1000, requestTime / 1000, delayTime);
    }

    void WriteTxt(double time, double serverExecuteTime, double requestTime, string txtText)
    {
        string path = "Assets/DelayTime.txt";
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);
        if (!File.Exists(path))
        {
            sw = fi.CreateText();
        }
        else
        {
            sw = fi.AppendText();
        }
        sw.WriteLine("Click: " + time + "| Request: " + requestTime + "| bash code: " + serverExecuteTime + "| The delay time: " + txtText);
        sw.Close();
        sw.Dispose();

    }

    void ReadTxt()
    {
        string path = "Assets/DelayTime.txt";
        List<string> delayTimes = new List<string>();
        StreamReader sr = new StreamReader(path);
        while (sr.Peek() >= 0)
        {
            delayTimes.Add(sr.ReadLine());
        }
        double total = 0.0;
        foreach (var x in delayTimes)
        {
            double.TryParse(x.Split(':', '|')[7], out double xx);
            total += xx;
        }
        Debug.Log("total/Count: " + total / delayTimes.Count);
    }
}
