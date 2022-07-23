using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Networking;
using System.IO;

public class RetrieveData : MonoBehaviour
{
    public Panel panel;
    [SerializeField] private string endpoint = "http://192.168.1.14:8000";

    private void OnMouseDown()
    {
        string id = this.transform.parent.name;

        Debug.Log("The " + this.name + " was clicked");
        Debug.Log("The " + this.transform.parent.name + " was clicked");
        StartCoroutine(panel.GetEffectId($"{endpoint}/getDeviceEffectsId/", id));

    }

}
