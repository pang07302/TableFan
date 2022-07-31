using System.Collections;
using UnityEngine;
public class PanelClose : MonoBehaviour
{
    public GameObject Panel;
    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }
}