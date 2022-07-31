using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.IO;
using Task = System.Threading.Tasks;
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
    public ToggleSwitch toggle;
    public PanelClose panelClose;
    public Service service;
    bool getServiceResponseData = false;
    static long clickTime;
    void Start()
    {
        ReadTxt();
    }
    private void OnMouseDown()
    {
        if (toggle.isPlay) // if toggle switch is play, click button will play the fan
        {
            clickTime = UnixNanoseconds(); // record the click time
            Play();
        }
        else // if toggle switch is create, click button allow to create effect
        {
            string device = this.transform.parent.name;
            panel.CreateDefaultEffect("Haptic", device, this.name);
        }
    }
    private void OnMouseUp()
    {
        if (this.name == "btn_off") { Bounce(); }
    }
    void Update()
    {
        if (toggle.isPlay)
        {
            panelClose.ClosePanel();
        }

        if (service.responseCode == 200 && getServiceResponseData)
        {
            MeasureTime(service.responseText, service.beforeRequestTime);
            service.setResponseCode(0);
            getServiceResponseData = !getServiceResponseData;
        }
    }
    void Play()
    {
        BounceAll();
        transform.DOLocalMoveY(-0.3f, 0.1f);
        blade.SetSpeed(speed);
        string fan = JsonUtility.ToJson(new Devices(8, this.name));
        if (this.name == "btn_off")
        {
            BounceAll();
            audiosource.PlayOneShot(pushDown);
        }
        else
        {
            audiosource.PlayOneShot(push);
        }
        service.SendDeploymentReq($"{service.endpoint}/fans", service.ToByteArray(fan));
        getServiceResponseData = !getServiceResponseData;
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
    public void MeasureTime(string responseText, long beforeRequestTime)
    {
        double.TryParse(responseText.Split(',', ':')[3], out double serverProcessingTime);
        long.TryParse(responseText.Split(',', ':')[5], out long afterRequestTime);
        double clickToSendTime = (double)(beforeRequestTime - clickTime);
        double requestTime = (double)(afterRequestTime - beforeRequestTime);
        double delayTime = clickToSendTime + serverProcessingTime + requestTime;
        WriteTxt(clickToSendTime / 1e9, serverProcessingTime / 1e9, requestTime / 1e9, delayTime / 1e9);
    }
    void WriteTxt(double clickToSendTime, double serverProcessingTime, double requestTime, double delayTime)
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
        Debug.Log("Average of deployment: " + total / delayTimes.Count);
    }
    long UnixNanoseconds()
    {
        System.DateTime unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - unixStart).Ticks * 100;
    }
}
