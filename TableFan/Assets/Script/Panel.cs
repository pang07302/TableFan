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
    [SerializeField] private TMP_Text deviceName;
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
    static bool setCategoryOption = false;
    static string operation;
    static string effectId;
    public ToggleSwitch toggle;
    [SerializeField] private string endpoint = "http://192.168.1.14:8000";

    void FillEffect(int index)
    {
        switch (category.options[index].text)
        {
            case "Sight":
                if (sightEffect == null) { CreateDefaultSight(); }
                FillSightEffect();
                print("sight");
                break;
            case "Audio":
                if (audioEffect == null) { CreateDefaultAudio(); }
                FillAudioEffect();
                print("audio");
                break;
            case "Haptic":
                if (hapticEffect == null) { CreateDefaultHaptic(); }
                FillHapticEffect();
                print("haptic");
                break;
            case "Smell":
                if (smellEffect == null) { CreateDefaultSmell(); }
                FillSmellEffect();
                print("smell");
                break;
            case "Taste":
                if (tasteEffect == null) { CreateDefaultTaste(); }
                print("taste");
                FillTasteEffect();
                break;
        }
    }
    void Start()
    {
        CreateDefaultSight();
        CreateDefaultAudio();
        CreateDefaultHaptic();
        CreateDefaultSmell();
        CreateDefaultTaste();
        category.onValueChanged.AddListener(FillEffect);
    }
    void Update()
    {
        if (setCategoryOption)
        {
            SetCategoryOption();
            setCategoryOption = false;
        }
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
    void SetCategoryOption()
    {
        List<string> list = new List<string>(categoryOption.Split(','));
        category.options.Clear();
        foreach (string option in list)
        {
            category.options.Add(new TMP_Dropdown.OptionData(option));
        }
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;
    }
    void FillPanel()
    {
        title.text = operation = "Manage";
        SetCategoryOption();
        switch (category.options[category.value].text)
        {
            case "Sight":
                FillSightEffect();
                control.text = sightEffect.control;
                break;
            case "Audio":
                FillAudioEffect();
                control.text = audioEffect.control;
                break;
            case "Haptic":
                FillHapticEffect();
                control.text = hapticEffect.control;
                break;
            case "Smell":
                FillSmellEffect();
                control.text = smellEffect.control;
                break;
            case "Taste":
                FillTasteEffect();
                control.text = tasteEffect.control;
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
                    if (GUI.Button(new Rect(100, 35 * (i + 1), 300, 30), new GUIContent(idArray[i].Split('_')[1] +
                     "effect" + (i + 1)), customButton))
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
                SubmitSightEffect();
                submitedEffect = JsonUtility.ToJson(sightEffect);
                break;
            case "Audio":
                SubmitAudioEffect();
                submitedEffect = JsonUtility.ToJson(audioEffect);
                break;
            case "Haptic":
                SubmitHapticEffect();
                submitedEffect = JsonUtility.ToJson(hapticEffect);
                break;
            case "Smell":
                SubmitSmellEffect();
                submitedEffect = JsonUtility.ToJson(smellEffect);
                break;
            case "Taste":
                SubmitTasteEffect();
                submitedEffect = JsonUtility.ToJson(tasteEffect);
                break;
        }
        string id = operation == "Create" ? deviceName.text : effectId;
        SendSubmitEffect($"{endpoint}/{operation}Effect/" + id, ToByteArray(submitedEffect));
        panel.SetActive(false);
    }
    public byte[] ToByteArray(string json)
    {
        return new System.Text.UTF8Encoding().GetBytes(json);
    }
    void CreateDefaultSight()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effect.json");
        sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
        sightEffect.control = control.text;
        sightEffect.deviceId = deviceName.text;

    }
    void CreateDefaultAudio()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Audio_effect.json");
        audioEffect = JsonUtility.FromJson<AudioName.Audio>(defaultJson);
        audioEffect.control = control.text;
        audioEffect.deviceId = deviceName.text;

    }
    void CreateDefaultHaptic()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effect.json");
        hapticEffect = JsonUtility.FromJson<HapticName.Haptic>(defaultJson);
        hapticEffect.control = control.text;
        hapticEffect.deviceId = deviceName.text;
    }
    void CreateDefaultSmell()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Smell_effect.json");
        smellEffect = JsonUtility.FromJson<SmellName.Smell>(defaultJson);
        smellEffect.control = control.text;
        smellEffect.deviceId = deviceName.text;

    }
    void CreateDefaultTaste()
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Taste_effect.json");
        tasteEffect = JsonUtility.FromJson<TasteName.Taste>(defaultJson);
        tasteEffect.control = control.text;
        tasteEffect.deviceId = deviceName.text;
    }
    public void CreateDefaultEffect(string type, string device, string controlName)
    {
        control.text = controlName;
        deviceName.text = device;
        categoryName = type;
        categoryOption = type;
        setCategoryOption = true;
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;
        switch (categoryName)
        {
            case "Sight":
                CreateDefaultSight();
                FillSightEffect();
                break;
            case "Audio":
                CreateDefaultAudio();
                FillAudioEffect();
                break;
            case "Haptic":
                CreateDefaultHaptic();
                FillHapticEffect();
                break;
            case "Smell":
                CreateDefaultSmell();
                FillSmellEffect();
                break;
            case "Taste":
                CreateDefaultTaste();
                FillTasteEffect();
                break;
        }
        panel.SetActive(true);
        title.text = operation = "Create";
    }
    public async void GetEffectId(string address, string device)
    {
        deviceName.text = device;
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
        categoryName = req.Split('_')[1];
        UnityWebRequest request = UnityWebRequest.Get(address + req);
        var operation = request.SendWebRequest();
        while (!operation.isDone) await Task.Yield();
        string json = request.downloadHandler.text;
        switch (categoryName)
        {
            case "Sight":
                sightEffect = JsonUtility.FromJson<SightName.Sight>(json);
                break;
            case "Audio":
                audioEffect = JsonUtility.FromJson<AudioName.Audio>(json);
                break;
            case "Haptic":
                hapticEffect = JsonUtility.FromJson<HapticName.Haptic>(json);
                break;
            case "Smell":
                smellEffect = JsonUtility.FromJson<SmellName.Smell>(json);
                break;
            case "Taste":
                tasteEffect = JsonUtility.FromJson<TasteName.Taste>(json);
                break;
        }
        fillPanel = true;
    }
    public async void SendSubmitEffect(string address, byte[] effect)
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
    // Fill Effect 
    void FillSightEffect()
    {
        propertyType.text = sightEffect.description.properties.type;
        measure.text = sightEffect.description.properties.measure;
        unit.text = sightEffect.description.properties.unit;
        quantity.text = "" + sightEffect.description.properties.quantity;
        propertyId.text = sightEffect.description.properties.id;
        patternType.text = sightEffect.description.pattern.type;
        length.text = "" + sightEffect.description.pattern.LengthMs;
    }
    void FillAudioEffect()
    {
        propertyType.text = audioEffect.description.properties.type;
        measure.text = audioEffect.description.properties.measure;
        unit.text = audioEffect.description.properties.unit;
        quantity.text = "" + audioEffect.description.properties.quantity;
        patternType.text = audioEffect.description.pattern.type;
        length.text = "" + audioEffect.description.pattern.LengthMs;

    }
    void FillHapticEffect()
    {
        propertyType.text = hapticEffect.description.properties.type;
        measure.text = hapticEffect.description.properties.measure;
        unit.text = hapticEffect.description.properties.unit;
        quantity.text = "" + hapticEffect.description.properties.quantity;
        patternType.text = hapticEffect.description.pattern.type;
        length.text = "" + hapticEffect.description.pattern.LengthMs;
    }
    void FillSmellEffect()
    {
        propertyType.text = smellEffect.description.properties.type;
        measure.text = smellEffect.description.properties.measure;
        unit.text = smellEffect.description.properties.unit;
        quantity.text = "" + smellEffect.description.properties.quantity;
        patternType.text = smellEffect.description.pattern.type;
        length.text = "" + smellEffect.description.pattern.LengthMs;

    }
    void FillTasteEffect()
    {
        propertyType.text = tasteEffect.description.properties.type;
        measure.text = tasteEffect.description.properties.measure;
        unit.text = tasteEffect.description.properties.unit;
        quantity.text = "" + tasteEffect.description.properties.quantity;
        patternType.text = tasteEffect.description.pattern.type;
        length.text = "" + tasteEffect.description.pattern.LengthMs;

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
        sightEffect.deviceId = deviceName.text;
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
        audioEffect.deviceId = deviceName.text;
        audioEffect.control = control.text;
    }
    public void SubmitHapticEffect()
    {
        print(hapticEffect == null);
        hapticEffect.description.properties.type = propertyType.text;
        hapticEffect.description.properties.measure = measure.text;
        hapticEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); hapticEffect.description.properties.quantity = q;
        hapticEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); hapticEffect.description.pattern.LengthMs = l;
        hapticEffect.deviceId = deviceName.text;
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
        smellEffect.deviceId = deviceName.text;
        smellEffect.control = control.text;
    }
    public void SubmitTasteEffect()
    {
        tasteEffect.description.properties.type = propertyType.text;
        tasteEffect.description.properties.measure = measure.text;
        tasteEffect.description.properties.unit = unit.text;
        int.TryParse(quantity.text, out int q); tasteEffect.description.properties.quantity = q;
        tasteEffect.description.pattern.type = patternType.text;
        int.TryParse(length.text, out int l); tasteEffect.description.pattern.LengthMs = l;
        tasteEffect.deviceId = deviceName.text;
        tasteEffect.control = control.text;
    }
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        print(output);
    }

}


