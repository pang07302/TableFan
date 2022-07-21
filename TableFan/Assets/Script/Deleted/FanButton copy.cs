// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using UnityEngine.Networking;
// using System.IO;
// using System.Diagnostics;
// using Debug = UnityEngine.Debug;

// public class FanButton : MonoBehaviour
// {
//     public List<FanButton> fanbtns = new List<FanButton>();
//     public BtnOff btnOff;
//     public float speed;
//     public Blade blade;
//     public AudioSource audiosource;
//     public AudioClip push;
//     public Panel panel;
//     public CreateDevice service = new CreateDevice();
//     Stopwatch sw = new Stopwatch();
//     static string retrieveTime;
//     private double time;
//     static bool initial = true;

//     // public AudioClip bounce;
//     // [SerializeField] private string authericationEndpoint = "http://localhost:8000/fan";
//     // void Start()
//     // {
//     //     GenerateDefaultTable();
//     // }
//     public void BounceAll()
//     {
//         foreach (var item in fanbtns)
//         {
//             item.Bounce();
//         }
//     }


//     public void GenerateDefaultTable()
//     {
//         string deviceTable = System.IO.File.ReadAllText("Assets/Json/DeviceTable.json");
//         StartCoroutine(service.SendReq("http://localhost:8000/createDeviceTable", service.ToByteArray(deviceTable)));
//         string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
//         StartCoroutine(service.SendReq("http://localhost:8000/createEffectTable", service.ToByteArray(effectTable)));
//         // string effectTable = System.IO.File.ReadAllText("Assets/Json/EffectTable.json");
//     }

//     private void OnMouseDown()
//     {
//         sw.Start();
//         if (initial)
//         {
//             GenerateDefaultTable();
//             initial = false;
//         }

//         BounceAll();
//         transform.DOLocalMoveY(-0.3f, 0.1f);
//         audiosource.PlayOneShot(push);


//         blade.SetSpeed(speed);

//         string fan;
//         if (speed = 0)
//         {
//             BounceAll();
//             fan = JsonUtility.ToJson(new Devices(1, "Off"));
//         }
//         else
//         {
//             fan = JsonUtility.ToJson(new Devices(1, "On"));
//         }

//         StartCoroutine(SendReq("http://localhost:8000/fans", service.ToByteArray(fan)));
//         string id = "fan01"; // device id
//         string haptic_effects = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");

//         // StartCoroutine(SendReq("http://localhost:8000/haptic/" + id, ToByteArray(haptic_effects)));

//         panel.CreateDefaultEffect("Haptic", id, speed, null);



//         // StartCoroutine(SendReq("http://192.168.1.14:8000/fans", fan.generateDevice(1)));

//         // StartCoroutine(SendReq("http://192.168.1.14:8000/fan/", "On"));
//     }
//     private void OnMouseUp()
//     {
//         if (speed = 0) { Bounce(); }
//     }
//     public void Bounce()
//     {
//         transform.DOLocalMoveY(0f, 0.1f);
//         if (speed = 0) { audiosource.PlayOneShot(bounceUp); }
//     }



//     public IEnumerator SendReq(string address, byte[] req)
//     {
//         UnityWebRequest request = UnityWebRequest.Get(address);
//         request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
//         request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//         request.SetRequestHeader("content-Type", "application/json");
//         request.SetRequestHeader("Accept", "application/json");
//         request.SetRequestHeader("api-version", "0.1");
//         sw.Stop();

//         yield return request.SendWebRequest();
//         retrieveTime = request.downloadHandler.text.Split(',', ':')[3];
//         MeasureTime(retrieveTime);
//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError(request.error);
//         }
//         else
//         {
//             Debug.Log(request.downloadHandler.text);
//         }
//     }

//     public void MeasureTime(string retrieveTime)
//     {
//         time = sw.ElapsedMilliseconds;
//         time /= 1000;


//         double.TryParse(retrieveTime, out double rTime);
//         Debug.Log(time + ", " + rTime);
//         string delayTime = time + rTime + "";
//         WriteTxt(delayTime);


//     }

//     void WriteTxt(string txtText)
//     {
//         string path = "Assets/DelayTime.txt";
//         StreamWriter sw;
//         FileInfo fi = new FileInfo(path);
//         if (!File.Exists(path))
//         {
//             sw = fi.CreateText();
//         }
//         else
//         {
//             sw = fi.AppendText();
//         }
//         sw.WriteLine("The delay time: " + txtText);
//         sw.Close();
//         sw.Dispose();

//     }
// }
