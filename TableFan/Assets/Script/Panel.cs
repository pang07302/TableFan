using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;
public class Panel : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Dropdown category;
    [SerializeField] private TMP_InputField propertyType;
    [SerializeField] private TMP_InputField measure;
    [SerializeField] private TMP_InputField unit;
    [SerializeField] private TMP_InputField quantity;
    [SerializeField] private TMP_InputField propertyId;
    [SerializeField] private TMP_InputField patternType;
    [SerializeField] private TMP_InputField length;
    [SerializeField] private TMP_InputField control;
    public GameObject propertyIdParent;
    static int categoryIndex;
    static string categoryName;
    static string categoryOption;
    static string retrieveId;
    static string[] idArray;
    HapticName.Haptic hapticEffect;
    SightName.Sight sightEffect;
    AudioName.Audio audioEffect;
    SmellName.Smell smellEffect;
    TasteName.Taste tasteEffect;
    static bool fillPanel = false;
    static bool drawGUI = false;
    static string deviceId;
    static string operation;
    static string effectId;
    public ToggleSwitch toggle;
    [SerializeField] private string endpoint = "http://localhost:8000";

    void Start()
    {
        CreateEffectClass();
    }


    void Update()
    {
        if (toggle.isPlay) // if toggle switch is "Play", stop drawing the retrieved effects GUI box
        {
            drawGUI = false;
        }
        if (fillPanel) // if clicking the effect GUI box, the effect data is retrieved and then fill retrieved effect data to panel 
        {
            FillPanel();
            fillPanel = false; // stop filling
        }
        // if the category dropbox value is Sight/Smell/Taste, show the correspond content panel
        if (category.options[category.value].text == "Sight")
        {
            propertyIdParent.SetActive(true);
        }
        else
        {
            propertyIdParent.SetActive(false);

        }

    }

    void FillPanel()
    {
        title.text = operation = "Manage";
        List<string> list = new List<string>(categoryOption.Split(','));
        category.options.Clear();
        foreach (string option in list)
        {
            category.options.Add(new TMP_Dropdown.OptionData(option));
        }
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;

        switch (category.options[category.value].text)
        {
            case "Sight":
                RetrieveSightEffect();
                break;
            case "Audio":
                RetrieveAudioEffect();
                break;
            case "Haptic":
                RetrieveHapticEffect();
                break;
            case "Smell":
                RetrieveSmellEffect();
                break;
            case "Taste":
                RetrieveTasteEffect();
                break;

        }
        panel.SetActive(true);
    }

    void OnGUI()
    {

        if (drawGUI)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                GUIStyle customButton = new GUIStyle("button");
                customButton.fontSize = 12;
                if (idArray[i].Length > 2)
                    if (GUI.Button(new Rect(100, 35 * (i + 1), 300, 30), new GUIContent("effect" + (i + 1)), customButton))
                    {
                        effectId = idArray[i].Substring(1, idArray[i].Length - 2);
                        GetEffect($"{endpoint}/getDeviceEffect/", idArray[i].Substring(1, idArray[i].Length - 2));
                        drawGUI = false;

                    }
            }
        }
    }


    public void submit()
    {
        string submitedEffect = "";
        switch (category.options[category.value].text)
        {
            case "Sight":
                CreateEffectClass();
                SubmitSightEffect();
                submitedEffect = JsonUtility.ToJson(sightEffect);
                break;
            case "Audio":
                CreateEffectClass();
                SubmitAudioEffect();
                submitedEffect = JsonUtility.ToJson(audioEffect);
                break;
            case "Haptic":
                CreateEffectClass();
                SubmitHapticEffect();
                submitedEffect = JsonUtility.ToJson(hapticEffect);
                break;
            case "Smell":
                CreateEffectClass();
                SubmitSmellEffect();
                submitedEffect = JsonUtility.ToJson(smellEffect);
                break;
            case "Taste":
                CreateEffectClass();
                SubmitTasteEffect();
                submitedEffect = JsonUtility.ToJson(tasteEffect);
                break;
        }

        string id = operation == "Create" ? deviceId : effectId;
        SubmitEffect($"{endpoint}/{operation}Effect/" + id, ToByteArray(submitedEffect));
        panel.SetActive(false);
    }

    public byte[] ToByteArray(string json)
    {
        return new System.Text.UTF8Encoding().GetBytes(json);
    }

    void CreateEffectClass()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effect.json");
        sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
        defaultJson = System.IO.File.ReadAllText("Assets/Json/Audio_effect.json");
        audioEffect = JsonUtility.FromJson<AudioName.Audio>(defaultJson);
        defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effect.json");
        hapticEffect = JsonUtility.FromJson<HapticName.Haptic>(defaultJson);
        defaultJson = System.IO.File.ReadAllText("Assets/Json/Smell_effect.json");
        smellEffect = JsonUtility.FromJson<SmellName.Smell>(defaultJson);
        defaultJson = System.IO.File.ReadAllText("Assets/Json/Taste_effect.json");
        tasteEffect = JsonUtility.FromJson<TasteName.Taste>(defaultJson);
    }



    void CreateDefaultSight(string controlName)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effect.json");
        sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
        sightEffect.control = controlName;
        RetrieveSightEffect();
    }
    void CreateDefaultAudio(string controlName)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Audio_effect.json");
        audioEffect = JsonUtility.FromJson<AudioName.Audio>(defaultJson);
        audioEffect.control = controlName;
        RetrieveAudioEffect();
    }

    void CreateDefaultHaptic(string controlName)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effect.json");
        hapticEffect = JsonUtility.FromJson<HapticName.Haptic>(defaultJson);
        hapticEffect.control = controlName;
        DumpToConsole(hapticEffect);
        RetrieveHapticEffect();
    }

    void CreateDefaultSmell(string controlName)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Smell_effect.json");
        smellEffect = JsonUtility.FromJson<SmellName.Smell>(defaultJson);
        smellEffect.control = controlName;
        RetrieveSmellEffect();
    }
    void CreateDefaultTaste(string controlName)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Taste_effect.json");
        tasteEffect = JsonUtility.FromJson<TasteName.Taste>(defaultJson);
        tasteEffect.control = controlName;
        RetrieveTasteEffect();
    }

    public void CreateDefaultEffect(string type, string id, string controlName)
    {
        print(controlName);
        deviceId = id;
        categoryName = type;
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;

        switch (categoryName)
        {
            case "Sight":
                CreateDefaultSight(controlName);
                break;
            case "Audio":
                CreateDefaultAudio(controlName);
                break;
            case "Haptic":
                CreateDefaultHaptic(controlName);
                break;
            case "Smell":
                CreateDefaultSmell(controlName);
                break;
            case "Taste":
                CreateDefaultTaste(controlName);
                break;
        }
        panel.SetActive(true);
        title.text = operation = "Create";
    }


    public async void GetEffectId(string address, string device)
    {
        deviceId = device;
        UnityWebRequest request = UnityWebRequest.Get(address + device);
        var operation = request.SendWebRequest();
        while (!operation.isDone) await Task.Yield();


        string resText = request.downloadHandler.text.Substring(1, request.downloadHandler.text.Length - 3);
        if (request.responseCode == 200)
        {
            retrieveId = resText.Split('|')[0];
            retrieveId = retrieveId.Substring(0, retrieveId.Length - 2);
            categoryOption = resText.Split('|')[1];
            idArray = retrieveId.Split(',');
            drawGUI = true;
        }
    }

    public async void GetEffect(string address, string req)
    {
        print(req);
        categoryName = req.Split('_')[1];
        UnityWebRequest request = UnityWebRequest.Get(address + req);
        var operation = request.SendWebRequest();
        while (!operation.isDone) await Task.Yield();

        string json = request.downloadHandler.text;
        hapticEffect = JsonUtility.FromJson<HapticName.Haptic>(json);
        audioEffect = JsonUtility.FromJson<AudioName.Audio>(json);
        sightEffect = JsonUtility.FromJson<SightName.Sight>(json);
        smellEffect = JsonUtility.FromJson<SmellName.Smell>(json);
        tasteEffect = JsonUtility.FromJson<TasteName.Taste>(json);
        fillPanel = true;
    }

    public async void SubmitEffect(string address, byte[] effect)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(effect);
        request.SetRequestHeader("content-Type", "application/json");

        var operation = request.SendWebRequest();
        while (!operation.isDone) await Task.Yield();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }




    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    // Retrieve Effect 

    void RetrieveSightEffect()
    {
        propertyType.text = sightEffect.description.properties.type;
        measure.text = sightEffect.description.properties.measure;
        unit.text = sightEffect.description.properties.unit;
        quantity.text = "" + sightEffect.description.properties.quantity;
        propertyId.text = sightEffect.description.properties.id;
        patternType.text = sightEffect.description.pattern.type;
        length.text = "" + sightEffect.description.pattern.LengthMs;
        control.text = sightEffect.control;

    }
    void RetrieveAudioEffect()
    {
        propertyType.text = audioEffect.description.properties.type;
        measure.text = audioEffect.description.properties.measure;
        unit.text = audioEffect.description.properties.unit;
        quantity.text = "" + audioEffect.description.properties.quantity;
        patternType.text = audioEffect.description.pattern.type;
        length.text = "" + audioEffect.description.pattern.LengthMs;
        control.text = audioEffect.control;
    }
    void RetrieveHapticEffect()
    {
        propertyType.text = hapticEffect.description.properties.type;
        measure.text = hapticEffect.description.properties.measure;
        unit.text = hapticEffect.description.properties.unit;
        quantity.text = "" + hapticEffect.description.properties.quantity;
        patternType.text = hapticEffect.description.pattern.type;
        length.text = "" + hapticEffect.description.pattern.LengthMs;
        print(hapticEffect.control);
        control.text = hapticEffect.control;
    }
    void RetrieveSmellEffect()
    {
        propertyType.text = smellEffect.description.properties.type;
        measure.text = smellEffect.description.properties.measure;
        unit.text = smellEffect.description.properties.unit;
        quantity.text = "" + smellEffect.description.properties.quantity;
        patternType.text = smellEffect.description.pattern.type;
        length.text = "" + smellEffect.description.pattern.LengthMs;
        control.text = smellEffect.control;
    }
    void RetrieveTasteEffect()
    {
        propertyType.text = tasteEffect.description.properties.type;
        measure.text = tasteEffect.description.properties.measure;
        unit.text = tasteEffect.description.properties.unit;
        quantity.text = "" + tasteEffect.description.properties.quantity;
        patternType.text = tasteEffect.description.pattern.type;
        length.text = "" + tasteEffect.description.pattern.LengthMs;
        control.text = tasteEffect.control;
    }

    //Submit Effect
    public void SubmitSightEffect()
    {
        sightEffect.description.properties.type = propertyType.text;
        sightEffect.description.properties.measure = measure.text;
        sightEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); sightEffect.description.properties.quantity = q;
        sightEffect.description.properties.id = propertyId.text;
        sightEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); sightEffect.description.pattern.LengthMs = l;
        sightEffect.deviceId = deviceId;
        sightEffect.control = control.text;
    }
    public void SubmitAudioEffect()
    {
        audioEffect.description.properties.type = propertyType.text;
        audioEffect.description.properties.measure = measure.text;
        audioEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); audioEffect.description.properties.quantity = q;
        audioEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); audioEffect.description.pattern.LengthMs = l;
        audioEffect.deviceId = deviceId;
        audioEffect.control = control.text;
    }
    public void SubmitHapticEffect()
    {
        hapticEffect.description.properties.type = propertyType.text;
        hapticEffect.description.properties.measure = measure.text;
        hapticEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); hapticEffect.description.properties.quantity = q;
        hapticEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); hapticEffect.description.pattern.LengthMs = l;
        hapticEffect.deviceId = deviceId;
        hapticEffect.control = control.text;
    }
    public void SubmitSmellEffect()
    {
        smellEffect.description.properties.type = propertyType.text;
        smellEffect.description.properties.measure = measure.text;
        smellEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); smellEffect.description.properties.quantity = q;
        smellEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); smellEffect.description.pattern.LengthMs = l;
        smellEffect.control = control.text;
        smellEffect.deviceId = deviceId;
    }
    public void SubmitTasteEffect()
    {
        tasteEffect.description.properties.type = propertyType.text;
        tasteEffect.description.properties.measure = measure.text;
        tasteEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); tasteEffect.description.properties.quantity = q;
        tasteEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); tasteEffect.description.pattern.LengthMs = l;
        tasteEffect.deviceId = deviceId;
        tasteEffect.control = control.text;
    }


}


