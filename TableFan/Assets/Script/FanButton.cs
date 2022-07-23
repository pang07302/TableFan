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
    static long clickTime;
    [SerializeField] private string endpoint = "http://192.168.1.14:8000";
    public ToggleSwitch toggle;
    public PanelClose panelClose;
    public Service service;
    bool flag = false;

    void Update(){
        if(toggle.isPlay){
            panelClose.ClosePanel();
        }
        if (service.responseCode==200 && flag)
        {
            MeasureTime(service.responseText, service.beforeRequestTime, service.afterResponseTime);
            service.setResponseCode(0);
            flag = !flag;
        }
    }
  
    private void OnMouseDown()
    {
        
      if(toggle.isPlay)
      {
        clickTime = UnixNanoseconds();
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

        StartCoroutine(service.SendReq($"{endpoint}/fans", service.ToByteArray(fan)));
        flag =!flag;
        // Debug.Log(service.responseCode);
        // Debug.Log(service.responseText);
        // if (service.responseCode == 200){
        //     MeasureTime(service.responseText, service.beforeRequestTime, service.afterResponseTime);

        // }
        
      }
      else
      {
        string id = "fan";
        string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");
        panel.CreateDefaultEffect("Haptic", id, this.name, speed, null);
      }
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

    public void MeasureTime(string responseText, long beforeRequestTime, long afterResponseTime)
    {
        double.TryParse(responseText.Split(',', ':')[3], out double serverProcessingTime);
        long.TryParse(responseText.Split(',', ':')[5], out long afterRequestTime);
        long.TryParse(responseText.Split(',', ':')[7], out long beforeResponseTime);
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
