using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine.Networking;
public class CreatePanel : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private TMP_Dropdown category;
    [SerializeField] private TMP_InputField start1;
    [SerializeField] private TMP_InputField start2;
    [SerializeField] private TMP_InputField start3;
    [SerializeField] private TMP_InputField start4;
    [SerializeField] private TMP_InputField pattern1;
    [SerializeField] private TMP_InputField pattern2;
    [SerializeField] private TMP_InputField pattern3;
    [SerializeField] private TMP_InputField pattern4;
    [SerializeField] private TMP_InputField length1;
    [SerializeField] private TMP_InputField length2;
    [SerializeField] private TMP_InputField length3;
    [SerializeField] private TMP_InputField length4;
    [SerializeField] private TMP_InputField rate1;
    [SerializeField] private TMP_InputField rate2;
    [SerializeField] private TMP_InputField rate3;
    [SerializeField] private TMP_InputField rate4;
    static int categoryIndex;
    static string categoryName;
    static string retrieveId;
    static string[] idArray;
    Haptic hapticEffect;
    SightName.Sight sightEffect;
    static bool view = false;
    static bool flag = false;




    void Start()
    {
    }


    void OnMouseDown()
    {

    }



    public void CreateDefaultEffect(string type, float frequency)
    {
        string defaultJson;
        categoryName = type;
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;

        switch (categoryName)
        {
            case "Sight":
                defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
                sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
                break;
            case "Audio":
                defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
                sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
                break;
            case "Haptic":

                defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");
                hapticEffect = JsonUtility.FromJson<Haptic>(defaultJson);
                hapticEffect.haptic_effects[0].description.rate.frequency = (int)frequency;
                hapticEffect.haptic_effects[1].description.rate.frequency = (int)frequency;
                hapticEffect.haptic_effects[2].description.rate.frequency = (int)frequency;
                hapticEffect.haptic_effects[3].description.rate.frequency = (int)frequency;
                panel.SetActive(true);

                break;
            case "Smell":
                defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
                sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
                break;
            case "Taste":
                defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
                sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
                break;
        }
        RetrieveHapticEffect();



        DumpToConsole(hapticEffect);

    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    // Retrieve Effect 
    void RetrieveHapticEffect()
    {
        start1.text = "" + hapticEffect.haptic_effects[0].start;
        start2.text = "" + hapticEffect.haptic_effects[1].start;
        start3.text = "" + hapticEffect.haptic_effects[2].start;
        start4.text = "" + hapticEffect.haptic_effects[3].start;
        pattern1.text = hapticEffect.haptic_effects[0].description.pattern[0].type;
        pattern2.text = hapticEffect.haptic_effects[1].description.pattern[0].type;
        pattern3.text = hapticEffect.haptic_effects[2].description.pattern[0].type;
        pattern4.text = hapticEffect.haptic_effects[3].description.pattern[0].type;
        length1.text = "" + hapticEffect.haptic_effects[0].description.pattern[0].LengthMs;
        length2.text = "" + hapticEffect.haptic_effects[1].description.pattern[0].LengthMs;
        length3.text = "" + hapticEffect.haptic_effects[2].description.pattern[0].LengthMs;
        length4.text = "" + hapticEffect.haptic_effects[3].description.pattern[0].LengthMs;
        rate1.text = "" + hapticEffect.haptic_effects[0].description.rate.frequency;
        rate2.text = "" + hapticEffect.haptic_effects[1].description.rate.frequency;
        rate3.text = "" + hapticEffect.haptic_effects[2].description.rate.frequency;
        rate4.text = "" + hapticEffect.haptic_effects[3].description.rate.frequency;
    }



    //Submit Effect

}


