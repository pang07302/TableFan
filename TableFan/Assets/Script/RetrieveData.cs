using UnityEngine;
public class RetrieveData : MonoBehaviour
{
    public Panel panel;
    public ToggleSwitch toggle;
    public Service service;
    private void OnMouseDown()
    {
        string device = this.transform.parent.name;

        if (!toggle.isPlay)
        {
            Debug.Log(service.endpoint);
            panel.GetEffectId($"{service.endpoint}/getDeviceEffectsId/", device);
        }
    }
}
