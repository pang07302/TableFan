using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class Service : MonoBehaviour
{
    [SerializeField] private long _responseCode;
    public long responseCode{ get{return _responseCode;}}
    public void setResponseCode(long code)
    {
        _responseCode = code;
    }
    [SerializeField] private string _responseText;
    public string responseText{ get{return _responseText;}}

    [SerializeField] private long _beforeRequestTime; 
    public long beforeRequestTime{ get{return _beforeRequestTime;}}
    [SerializeField] private long _afterResponseTime; 
    public long afterResponseTime{ get{return _afterResponseTime;}}
    

    public IEnumerator SendReq(string address, byte[] req)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(req);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("api-version", "0.1");
        _beforeRequestTime = UnixNanoseconds();
        Debug.Log(beforeRequestTime);
        yield return request.SendWebRequest();
        _afterResponseTime = UnixNanoseconds();
        _responseCode = request.responseCode;
        Debug.Log(_responseCode);
        _responseText = request.downloadHandler.text;
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public byte[] ToByteArray(string json)
    {
        return new System.Text.UTF8Encoding().GetBytes(json);
    }

    long UnixNanoseconds()
    {
        System.DateTime unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - unixStart).Ticks * 100;
    }
}


