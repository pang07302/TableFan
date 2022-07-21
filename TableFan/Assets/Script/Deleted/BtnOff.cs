// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using System.Diagnostics;
// using Debug = UnityEngine.Debug;

// public class BtnOff : MonoBehaviour
// {
//     public List<FanButton> fanbtns = new List<FanButton>();
//     public float speed;
//     public Blade blade;
//     public FanButton fanButton = new FanButton();
//     public AudioSource audiosource;
//     public AudioClip pushDown;
//     public AudioClip bounceUp;
//     public CreateDevice service = new CreateDevice();
//     Stopwatch sw = new Stopwatch();
//     static string retrieveTime;
//     private double time;

//     public void BounceAll()
//     {
//         foreach (var item in fanbtns)
//         {
//             item.Bounce();
//         }
//     }
//     private void OnMouseDown()
//     {
//         sw.Start();
//         transform.DOLocalMoveY(-0.3f, 0.1f);
//         audiosource.PlayOneShot(pushDown);
//         BounceAll();
//         blade.SetSpeed(speed);

//         string fan = JsonUtility.ToJson(new Devices(1, "Off"));
//         StartCoroutine(SendReq("localhost:8000/fan/", service.ToByteArray(fan)));
//         // StartCoroutine(fanButton.SendReq("http://149.157.109.61:8000/fan/", "Off"));
//         // StartCoroutine(fanButton.SendReq("http://192.168.1.14:8000/fan/", "Off"));
//     }

//     private void OnMouseUp()
//     {
//         Bounce();
//     }

//     public void Bounce()
//     {
//         transform.DOLocalMoveY(0f, 0.1f);
//         audiosource.PlayOneShot(bounceUp);
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
// }
