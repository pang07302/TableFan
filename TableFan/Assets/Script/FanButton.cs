using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;

public class FanButton : MonoBehaviour
{
    public BtnOff btnOff;
    public float speed;
    public Blade blade;
    public AudioSource audiosource;
    public AudioClip push;
    // public AudioClip bounce;
    // [SerializeField] private string authericationEndpoint = "http://localhost:8000/fan";


    private void OnMouseDown()
    {

        btnOff.BounceAll();
        transform.DOLocalMoveY(-0.3f, 0.1f);
        audiosource.PlayOneShot(push);
        blade.SetSpeed(speed);
        StartCoroutine(SendReq("http://192.168.1.14:8000/fan/", "On"));


    }
    public IEnumerator SendReq(string address, string req)
    {
        UnityWebRequest request = UnityWebRequest.Get(address + req);
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
