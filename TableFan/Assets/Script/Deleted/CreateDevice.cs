// using System.Collections;
// using UnityEngine;
// using UnityEngine.Networking;
// using System.IO;
// public class CreateDevice
// {
//     public byte[] ToByteArray(string json)
//     {
//         return new System.Text.UTF8Encoding().GetBytes(json);
//     }


//     public IEnumerator SendReq(string address, byte[] req)
//     {
//         UnityWebRequest request = UnityWebRequest.Get(address);
//         request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
//         request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
//         request.SetRequestHeader("content-Type", "application/json");
//         request.SetRequestHeader("Accept", "application/json");
//         request.SetRequestHeader("api-version", "0.1");
//         yield return request.SendWebRequest();
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

