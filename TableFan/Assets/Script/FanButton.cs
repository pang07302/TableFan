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
    [SerializeField] private string endpoint = "http://192.168.1.14:8000";

    private void OnMouseDown()
    {
        clickTime = UnixNanoseconds();
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

        StartCoroutine(SendReq($"{endpoint}/fans", service.ToByteArray(fan)));
        string id = "fan";
        string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");
        panel.CreateDefaultEffect("Haptic", id, this.name, speed, null);

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
    public void BounceAll()
    {
        foreach (var item in fanbtns)
        {
            item.Bounce();
        }
    }



    public IEnumerator SendReq(string address, byte[] req)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("api-version", "0.1");

        long beforeRequestTime = UnixNanoseconds();
        yield return request.SendWebRequest();
        long afterResponseTime = UnixNanoseconds();
        Debug.Log("before: " + beforeRequestTime + "after: " + afterResponseTime);


        if (request.responseCode == 200)
        {
            string serverProcessingTimeStr = request.downloadHandler.text.Split(',', ':')[3];
            string afterRequestTimeStr = request.downloadHandler.text.Split(',', ':')[5];
            string beforeResponseTimeStr = request.downloadHandler.text.Split(',', ':')[7];
            MeasureTime(serverProcessingTimeStr, afterRequestTimeStr, beforeResponseTimeStr, beforeRequestTime, afterResponseTime);
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



    public void GenerateDefaultTable()
    {
        string deviceTable = System.IO.File.ReadAllText("Assets/Json/DeviceTable.json");
        StartCoroutine(service.SendReq($"{endpoint}/createDeviceTable", service.ToByteArray(deviceTable)));
        string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
        StartCoroutine(service.SendReq($"{endpoint}/createEffectTable", service.ToByteArray(effectTable)));
        // string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
    }


    public void MeasureTime(string serverProcessingTimeStr, string afterRequestTimeStr, string beforeResponseTimeStr, long beforeRequestTime, long afterResponseTime)
    {
        Debug.Log(clickTime + " , " + beforeRequestTime + " , " + afterResponseTime);
        double.TryParse(serverProcessingTimeStr, out double serverProcessingTime);
        long.TryParse(afterRequestTimeStr, out long afterRequestTime);
        long.TryParse(beforeResponseTimeStr, out long beforeResponseTime);
        double clickToSendTime = (double)(beforeRequestTime - clickTime);
        double requestTime = (double)(afterRequestTime - beforeRequestTime);
        double responseTime = (double)(afterResponseTime - beforeResponseTime);
        string delayTime = (clickToSendTime + serverProcessingTime + afterRequestTime - beforeRequestTime) / 1e9 + "";
        WriteTxt(clickToSendTime / 1e9, serverProcessingTime / 1e9, requestTime / 1e9, delayTime);
    }

    void WriteTxt(double clickToSendTime, double serverProcessingTime, double requestTime, string delayTime)
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
        sw.WriteLine("Click: " + clickToSendTime + "| Request: " + requestTime + "| bash code: " + serverProcessingTime + "| The delay time: " + delayTime);
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
    long UnixNanoseconds()
    {
        System.DateTime unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        return (System.DateTime.UtcNow - unixStart).Ticks * 100;
    }
}
