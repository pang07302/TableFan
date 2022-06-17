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

    public void PopAll()
    {
        foreach (var item in fanbtns)
        {
            item.PopUp();
        }
    }
    private void OnMouseDown()
    {

        transform.DOLocalMoveY(-0.3f, 0.1f);
        PopAll();
        blade.SetSpeed(speed);

        StartCoroutine(fanButton.SendReq("http://192.168.1.14:8000/fan/", "Off"));



    }

    private void OnMouseUp()
    {
        PopUp();



    }

    public void PopUp()
    {
        transform.DOLocalMoveY(0f, 0.1f);
    }
}
