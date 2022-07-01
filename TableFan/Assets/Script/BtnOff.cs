using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnOff : MonoBehaviour
{
    public List<FanButton> fanbtns = new List<FanButton>();
    public float speed;
    public Blade blade;
    public FanButton fanButton = new FanButton();
    public AudioSource audiosource;
    public AudioClip pushDown;
    public AudioClip bounceUp;

    public void BounceAll()
    {
        foreach (var item in fanbtns)
        {
            item.Bounce();
        }
    }
    private void OnMouseDown()
    {

        transform.DOLocalMoveY(-0.3f, 0.1f);
        audiosource.PlayOneShot(pushDown);
        BounceAll();
        blade.SetSpeed(speed);
        // StartCoroutine(fanButton.SendReq("http://149.157.109.61:8000/fan/", "Off"));
        // StartCoroutine(fanButton.SendReq("http://192.168.1.14:8000/fan/", "Off"));
    }

    private void OnMouseUp()
    {
        Bounce();
    }

    public void Bounce()
    {
        transform.DOLocalMoveY(0f, 0.1f);
        audiosource.PlayOneShot(bounceUp);
    }
}
