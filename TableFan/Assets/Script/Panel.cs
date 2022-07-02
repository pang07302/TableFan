using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine.Networking;
public class Panel : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private TMP_Text title;
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
    public GameObject contentPanel;
    [SerializeField] private TMP_Text contentLabel;
    [SerializeField] private TMP_InputField content1;
    [SerializeField] private TMP_InputField content2;
    [SerializeField] private TMP_InputField content3;
    [SerializeField] private TMP_InputField content4;
    static int categoryIndex;
    static string categoryName;
    static string retrieveId;
    static string[] idArray;
    static Haptic hapticEffect;
    SightName.Sight sightEffect;
    AudioName.Audio audioEffect;
    SmellName.Smell smellEffect;
    TasteName.Taste tasteEffect;
    static bool view = false;
    static bool flag = false;
    static string deviceId;
    static string operation;
    static string effectId;




    void Start()
    {
        // string json = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
        // sightEffect = JsonUtility.FromJson<SightName.Sight>(json);
        // DumpToConsole(sightEffect);

    }
    void Update()
    {
        if (view)
        {
            title.text = operation = "Manage";
            categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
            category.value = categoryIndex;
            category.options[categoryIndex].text = categoryName;
            Debug.Log(categoryName);
            switch (category.value)
            {
                case 0:
                    contentPanel.SetActive(true);
                    contentLabel.text = "Colour:";
                    RetrieveSightEffect();
                    break;
                case 1:
                    contentPanel.SetActive(false);
                    break;
                case 2:
                    contentPanel.SetActive(false);
                    RetrieveHapticEffect();
                    break;

                case 3:
                    contentPanel.SetActive(true);
                    contentLabel.text = "Fragrance:";
                    break;
                case 4:
                    contentPanel.SetActive(true);
                    contentLabel.text = "Flavour:";
                    break;
                default: contentPanel.SetActive(false); contentLabel.text = ""; break;
            }
            panel.SetActive(true);
            view = false;
        }
        switch (category.value)
        {
            case 0:
                contentPanel.SetActive(true);
                contentLabel.text = "Colour:";
                break;
            case 3:
                contentPanel.SetActive(true);
                contentLabel.text = "Fragrance:";
                break;
            case 4:
                contentPanel.SetActive(true);
                contentLabel.text = "Flavour:";
                break;
            default: contentPanel.SetActive(false); contentLabel.text = ""; break;
        }

    }

    void ContentPanel()
    {

    }

    void OnMouseDown()
    {

    }
    void OnGUI()
    {

        if (flag)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                GUIStyle customButton = new GUIStyle("button");
                customButton.fontSize = 12;
                if (idArray[i].Length > 2)
                    if (GUI.Button(new Rect(100, 35 * (i + 1), 300, 30), new GUIContent("effect" + (i + 1)), customButton))
                    {
                        Debug.Log(idArray[i]);
                        effectId = idArray[i].Substring(1, idArray[i].Length - 2);
                        StartCoroutine(GetEffect("http://localhost:8000/getDeviceEffect/", idArray[i].Substring(1, idArray[i].Length - 2)));
                    }
            }
        }
    }

    public void submit()
    {
        string submitedEffect = "";
        Debug.Log(category.value);
        switch (category.value)
        {
            case 0:
                Debug.Log(sightEffect);

                if (sightEffect == null)
                {

                    string defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
                    sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);

                }
                SubmitSightEffect();
                DumpToConsole(sightEffect);
                submitedEffect = JsonUtility.ToJson(sightEffect);
                break;
            case 1:
                {
                    string defaultJson = System.IO.File.ReadAllText("Assets/Json/Audio_effects.json");
                    audioEffect = JsonUtility.FromJson<AudioName.Audio>(defaultJson);
                }
                SubmitAudioEffect();
                submitedEffect = JsonUtility.ToJson(audioEffect);
                break;
            case 2:
                {
                    string defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");
                    hapticEffect = JsonUtility.FromJson<Haptic>(defaultJson);
                }
                SubmitHapticEffect();
                submitedEffect = JsonUtility.ToJson(hapticEffect);
                break;
            case 3:
                {
                    string defaultJson = System.IO.File.ReadAllText("Assets/Json/Smell_effects.json");
                    smellEffect = JsonUtility.FromJson<SmellName.Smell>(defaultJson);
                }
                SubmitSmellEffect();
                submitedEffect = JsonUtility.ToJson(smellEffect);
                break;
            case 4:
                {
                    string defaultJson = System.IO.File.ReadAllText("Assets/Json/Taste_effects.json");
                    tasteEffect = JsonUtility.FromJson<TasteName.Taste>(defaultJson);
                }
                SubmitTasteEffect();
                submitedEffect = JsonUtility.ToJson(tasteEffect);
                break;
        }

        Debug.Log(submitedEffect);
        Debug.Log(operation);
        string id = operation=="Create"?deviceId:effectId;
        StartCoroutine(SubmitEffect($"http://localhost:8000/{operation}Effect/" + id, ToByteArray(submitedEffect)));
    }

    public byte[] ToByteArray(string json)
    {
        return new System.Text.UTF8Encoding().GetBytes(json);
    }

    void CreateDefaultSight(float frequency, string colour)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Sight_effects.json");
        sightEffect = JsonUtility.FromJson<SightName.Sight>(defaultJson);
        sightEffect.sight_effects[0].description.rate.frequency = (int)frequency;
        sightEffect.sight_effects[1].description.rate.frequency = (int)frequency;
        sightEffect.sight_effects[2].description.rate.frequency = (int)frequency;
        sightEffect.sight_effects[3].description.rate.frequency = (int)frequency;
        sightEffect.sight_effects[0].description.pattern[0].colour = colour;
        sightEffect.sight_effects[1].description.pattern[0].colour = colour;
        sightEffect.sight_effects[2].description.pattern[0].colour = colour;
        sightEffect.sight_effects[3].description.pattern[0].colour = colour;
        contentPanel.SetActive(true);
        RetrieveSightEffect();
    }
    void CreateDefaultAudio(float frequency)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Audio_effects.json");
        audioEffect = JsonUtility.FromJson<AudioName.Audio>(defaultJson);
        audioEffect.audio_effects[0].description.rate.frequency = (int)frequency;
        audioEffect.audio_effects[1].description.rate.frequency = (int)frequency;
        audioEffect.audio_effects[2].description.rate.frequency = (int)frequency;
        audioEffect.audio_effects[3].description.rate.frequency = (int)frequency;
        RetrieveAudioEffect();
    }
    void CreateDefaultHaptic(float frequency)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Haptic_effects.json");
        hapticEffect = JsonUtility.FromJson<Haptic>(defaultJson);
        hapticEffect.haptic_effects[0].description.rate.frequency = (int)frequency;
        hapticEffect.haptic_effects[1].description.rate.frequency = (int)frequency;
        hapticEffect.haptic_effects[2].description.rate.frequency = (int)frequency;
        hapticEffect.haptic_effects[3].description.rate.frequency = (int)frequency;
        RetrieveHapticEffect();
    }
    void CreateDefaultSmell(string fragrance)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Smell_effects.json");
        smellEffect = JsonUtility.FromJson<SmellName.Smell>(defaultJson);
        smellEffect.smell_effects[0].description.pattern[0].fragrance = fragrance;
        smellEffect.smell_effects[1].description.pattern[0].fragrance = fragrance;
        smellEffect.smell_effects[2].description.pattern[0].fragrance = fragrance;
        smellEffect.smell_effects[3].description.pattern[0].fragrance = fragrance;
        RetrieveSmellEffect();
    }
    void CreateDefaultTaste(string flavour)
    {
        string defaultJson = System.IO.File.ReadAllText("Assets/Json/Taste_effects.json");
        tasteEffect = JsonUtility.FromJson<TasteName.Taste>(defaultJson);
        tasteEffect.taste_effects[0].description.pattern[0].flavour = flavour;
        tasteEffect.taste_effects[1].description.pattern[0].flavour = flavour;
        tasteEffect.taste_effects[2].description.pattern[0].flavour = flavour;
        tasteEffect.taste_effects[3].description.pattern[0].flavour = flavour;
    }
    public void CreateDefaultEffect(string type, string id, float frequency, string content)
    {
        deviceId = id;
        categoryName = type;
        categoryIndex = category.options.FindIndex((i) => { return i.text.Equals(categoryName); });
        category.value = categoryIndex;
        category.options[categoryIndex].text = categoryName;
        Debug.Log(id);

        switch (categoryName)
        {
            case "Sight":
                if (content == null) content = "default"; CreateDefaultSight(frequency, content);
                break;
            case "Audio":
                CreateDefaultAudio(frequency);
                break;
            case "Haptic":
                CreateDefaultHaptic(frequency);
                break;
            case "Smell":
                if (content != null) CreateDefaultSmell(content);
                break;
            case "Taste":
                if (content != null) CreateDefaultTaste(content);
                break;
        }
        panel.SetActive(true);
        title.text = operation = "Create";
        DumpToConsole(hapticEffect);
    }

    public IEnumerator GetEffectId(string address, string id)
    {
        categoryName = id.Split('(', ')')[1];
        UnityWebRequest request = UnityWebRequest.Get(address + id);
        yield return request.SendWebRequest();
        retrieveId = request.downloadHandler.text.Substring(1, request.downloadHandler.text.Length - 2);
        idArray = retrieveId.Split(',');
        flag = true;
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }
    public IEnumerator GetEffect(string address, string id)
    {
        string req = id + '_' + categoryName;
        UnityWebRequest request = UnityWebRequest.Get(address + req);
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;

        hapticEffect = JsonUtility.FromJson<Haptic>(json);

        // DumpToConsole(hapticEffect);
        view = true;
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public IEnumerator SubmitEffect(string address, byte[] effect)
    {
        UnityWebRequest request = UnityWebRequest.Get(address);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(effect);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
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

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    // Retrieve Effect 
    void RetrieveSightEffect()
    {
        start1.text = "" + sightEffect.sight_effects[0].start;
        start2.text = "" + sightEffect.sight_effects[1].start;
        start3.text = "" + sightEffect.sight_effects[2].start;
        start4.text = "" + sightEffect.sight_effects[3].start;
        pattern1.text = sightEffect.sight_effects[0].description.pattern[0].type;
        pattern2.text = sightEffect.sight_effects[1].description.pattern[0].type;
        pattern3.text = sightEffect.sight_effects[2].description.pattern[0].type;
        pattern4.text = sightEffect.sight_effects[3].description.pattern[0].type;
        length1.text = "" + sightEffect.sight_effects[0].description.pattern[0].LengthMs;
        length2.text = "" + sightEffect.sight_effects[1].description.pattern[0].LengthMs;
        length3.text = "" + sightEffect.sight_effects[2].description.pattern[0].LengthMs;
        length4.text = "" + sightEffect.sight_effects[3].description.pattern[0].LengthMs;
        rate1.text = "" + sightEffect.sight_effects[0].description.rate.frequency;
        rate2.text = "" + sightEffect.sight_effects[1].description.rate.frequency;
        rate3.text = "" + sightEffect.sight_effects[2].description.rate.frequency;
        rate4.text = "" + sightEffect.sight_effects[3].description.rate.frequency;
        content1.text = "" + sightEffect.sight_effects[0].description.pattern[0].colour;
        content2.text = "" + sightEffect.sight_effects[1].description.pattern[0].colour;
        content3.text = "" + sightEffect.sight_effects[2].description.pattern[0].colour;
        content4.text = "" + sightEffect.sight_effects[3].description.pattern[0].colour;
    }
    void RetrieveAudioEffect()
    {
        start1.text = "" + audioEffect.audio_effects[0].start;
        start2.text = "" + audioEffect.audio_effects[1].start;
        start3.text = "" + audioEffect.audio_effects[2].start;
        start4.text = "" + audioEffect.audio_effects[3].start;
        pattern1.text = audioEffect.audio_effects[0].description.pattern[0].type;
        pattern2.text = audioEffect.audio_effects[1].description.pattern[0].type;
        pattern3.text = audioEffect.audio_effects[2].description.pattern[0].type;
        pattern4.text = audioEffect.audio_effects[3].description.pattern[0].type;
        length1.text = "" + audioEffect.audio_effects[0].description.pattern[0].LengthMs;
        length2.text = "" + audioEffect.audio_effects[1].description.pattern[0].LengthMs;
        length3.text = "" + audioEffect.audio_effects[2].description.pattern[0].LengthMs;
        length4.text = "" + audioEffect.audio_effects[3].description.pattern[0].LengthMs;
        rate1.text = "" + audioEffect.audio_effects[0].description.rate.frequency;
        rate2.text = "" + audioEffect.audio_effects[1].description.rate.frequency;
        rate3.text = "" + audioEffect.audio_effects[2].description.rate.frequency;
        rate4.text = "" + audioEffect.audio_effects[3].description.rate.frequency;
    }
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
    void RetrieveSmellEffect()
    {
        start1.text = "" + smellEffect.smell_effects[0].start;
        start2.text = "" + smellEffect.smell_effects[1].start;
        start3.text = "" + smellEffect.smell_effects[2].start;
        start4.text = "" + smellEffect.smell_effects[3].start;
        pattern1.text = smellEffect.smell_effects[0].description.pattern[0].type;
        pattern2.text = smellEffect.smell_effects[1].description.pattern[0].type;
        pattern3.text = smellEffect.smell_effects[2].description.pattern[0].type;
        pattern4.text = smellEffect.smell_effects[3].description.pattern[0].type;
        length1.text = "" + smellEffect.smell_effects[0].description.pattern[0].LengthMs;
        length2.text = "" + smellEffect.smell_effects[1].description.pattern[0].LengthMs;
        length3.text = "" + smellEffect.smell_effects[2].description.pattern[0].LengthMs;
        length4.text = "" + smellEffect.smell_effects[3].description.pattern[0].LengthMs;
        rate1.text = "" + smellEffect.smell_effects[0].description.rate.frequency;
        rate2.text = "" + smellEffect.smell_effects[1].description.rate.frequency;
        rate3.text = "" + smellEffect.smell_effects[2].description.rate.frequency;
        rate4.text = "" + smellEffect.smell_effects[3].description.rate.frequency;
        content1.text = "" + smellEffect.smell_effects[0].description.pattern[0].fragrance;
        content2.text = "" + smellEffect.smell_effects[1].description.pattern[0].fragrance;
        content3.text = "" + smellEffect.smell_effects[2].description.pattern[0].fragrance;
        content4.text = "" + smellEffect.smell_effects[3].description.pattern[0].fragrance;
    }
    void RetrieveTasteEffect()
    {
        start1.text = "" + tasteEffect.taste_effects[0].start;
        start2.text = "" + tasteEffect.taste_effects[1].start;
        start3.text = "" + tasteEffect.taste_effects[2].start;
        start4.text = "" + tasteEffect.taste_effects[3].start;
        pattern1.text = tasteEffect.taste_effects[0].description.pattern[0].type;
        pattern2.text = tasteEffect.taste_effects[1].description.pattern[0].type;
        pattern3.text = tasteEffect.taste_effects[2].description.pattern[0].type;
        pattern4.text = tasteEffect.taste_effects[3].description.pattern[0].type;
        length1.text = "" + tasteEffect.taste_effects[0].description.pattern[0].LengthMs;
        length2.text = "" + tasteEffect.taste_effects[1].description.pattern[0].LengthMs;
        length3.text = "" + tasteEffect.taste_effects[2].description.pattern[0].LengthMs;
        length4.text = "" + tasteEffect.taste_effects[3].description.pattern[0].LengthMs;
        rate1.text = "" + tasteEffect.taste_effects[0].description.rate.frequency;
        rate2.text = "" + tasteEffect.taste_effects[1].description.rate.frequency;
        rate3.text = "" + tasteEffect.taste_effects[2].description.rate.frequency;
        rate4.text = "" + tasteEffect.taste_effects[3].description.rate.frequency;
        content1.text = "" + tasteEffect.taste_effects[0].description.pattern[0].flavour;
        content2.text = "" + tasteEffect.taste_effects[1].description.pattern[0].flavour;
        content3.text = "" + tasteEffect.taste_effects[2].description.pattern[0].flavour;
        content4.text = "" + tasteEffect.taste_effects[3].description.pattern[0].flavour;
    }

    //Submit Effect
    public void SubmitSightEffect()
    {
        // parse to integer - start
        int.TryParse(start1.text, out int s1); sightEffect.sight_effects[0].start = s1;
        int.TryParse(start2.text, out int s2); sightEffect.sight_effects[1].start = s2;
        int.TryParse(start3.text, out int s3); sightEffect.sight_effects[2].start = s3;
        int.TryParse(start4.text, out int s4); sightEffect.sight_effects[3].start = s4;
        // pattern
        sightEffect.sight_effects[0].description.pattern[0].type = pattern1.text;
        sightEffect.sight_effects[1].description.pattern[0].type = pattern2.text;
        sightEffect.sight_effects[2].description.pattern[0].type = pattern3.text;
        sightEffect.sight_effects[3].description.pattern[0].type = pattern4.text;
        // parse to integer - length-ms
        int.TryParse(length1.text, out int l1); sightEffect.sight_effects[0].description.pattern[0].LengthMs = l1;
        int.TryParse(length2.text, out int l2); sightEffect.sight_effects[1].description.pattern[0].LengthMs = l2;
        int.TryParse(length3.text, out int l3); sightEffect.sight_effects[2].description.pattern[0].LengthMs = l3;
        int.TryParse(length4.text, out int l4); sightEffect.sight_effects[3].description.pattern[0].LengthMs = l4;
        // parse to integer - rate
        int.TryParse(rate1.text, out int r1); sightEffect.sight_effects[0].description.rate.frequency = r1;
        int.TryParse(rate2.text, out int r2); sightEffect.sight_effects[1].description.rate.frequency = r2;
        int.TryParse(rate3.text, out int r3); sightEffect.sight_effects[2].description.rate.frequency = r3;
        int.TryParse(rate4.text, out int r4); sightEffect.sight_effects[3].description.rate.frequency = r4;
        // colour
        sightEffect.sight_effects[0].description.pattern[0].colour = content1.text;
        sightEffect.sight_effects[1].description.pattern[0].colour = content2.text;
        sightEffect.sight_effects[2].description.pattern[0].colour = content3.text;
        sightEffect.sight_effects[3].description.pattern[0].colour = content4.text;
        // DumpToConsole(sightEffect);
    }
    public void SubmitAudioEffect()
    {
        // parse to integer - start
        int.TryParse(start1.text, out int s1); audioEffect.audio_effects[0].start = s1;
        int.TryParse(start2.text, out int s2); audioEffect.audio_effects[1].start = s2;
        int.TryParse(start3.text, out int s3); audioEffect.audio_effects[2].start = s3;
        int.TryParse(start4.text, out int s4); audioEffect.audio_effects[3].start = s4;
        // pattern
        audioEffect.audio_effects[0].description.pattern[0].type = pattern1.text;
        audioEffect.audio_effects[1].description.pattern[0].type = pattern2.text;
        audioEffect.audio_effects[2].description.pattern[0].type = pattern3.text;
        audioEffect.audio_effects[3].description.pattern[0].type = pattern4.text;
        // parse to integer - length-ms
        int.TryParse(length1.text, out int l1); audioEffect.audio_effects[0].description.pattern[0].LengthMs = l1;
        int.TryParse(length2.text, out int l2); audioEffect.audio_effects[1].description.pattern[0].LengthMs = l2;
        int.TryParse(length3.text, out int l3); audioEffect.audio_effects[2].description.pattern[0].LengthMs = l3;
        int.TryParse(length4.text, out int l4); audioEffect.audio_effects[3].description.pattern[0].LengthMs = l4;
        // parse to integer - rate
        int.TryParse(rate1.text, out int r1); audioEffect.audio_effects[0].description.rate.frequency = r1;
        int.TryParse(rate2.text, out int r2); audioEffect.audio_effects[1].description.rate.frequency = r2;
        int.TryParse(rate3.text, out int r3); audioEffect.audio_effects[2].description.rate.frequency = r3;
        int.TryParse(rate4.text, out int r4); audioEffect.audio_effects[3].description.rate.frequency = r4;
        DumpToConsole(audioEffect);
    }
    public void SubmitHapticEffect()
    {
        Debug.Log(hapticEffect.haptic_effects[0].start);
        // parse to integer - start
        int.TryParse(start1.text, out int s1); hapticEffect.haptic_effects[0].start = s1;
        int.TryParse(start2.text, out int s2); hapticEffect.haptic_effects[1].start = s2;
        int.TryParse(start3.text, out int s3); hapticEffect.haptic_effects[2].start = s3;
        int.TryParse(start4.text, out int s4); hapticEffect.haptic_effects[3].start = s4;
        // pattern
        hapticEffect.haptic_effects[0].description.pattern[0].type = pattern1.text;
        hapticEffect.haptic_effects[1].description.pattern[0].type = pattern2.text;
        hapticEffect.haptic_effects[2].description.pattern[0].type = pattern3.text;
        hapticEffect.haptic_effects[3].description.pattern[0].type = pattern4.text;
        // parse to integer - length-ms
        int.TryParse(length1.text, out int l1); hapticEffect.haptic_effects[0].description.pattern[0].LengthMs = l1;
        int.TryParse(length2.text, out int l2); hapticEffect.haptic_effects[1].description.pattern[0].LengthMs = l2;
        int.TryParse(length3.text, out int l3); hapticEffect.haptic_effects[2].description.pattern[0].LengthMs = l3;
        int.TryParse(length4.text, out int l4); hapticEffect.haptic_effects[3].description.pattern[0].LengthMs = l4;
        // parse to integer - rate
        int.TryParse(rate1.text, out int r1); hapticEffect.haptic_effects[0].description.rate.frequency = r1;
        int.TryParse(rate2.text, out int r2); hapticEffect.haptic_effects[1].description.rate.frequency = r2;
        int.TryParse(rate3.text, out int r3); hapticEffect.haptic_effects[2].description.rate.frequency = r3;
        int.TryParse(rate4.text, out int r4); hapticEffect.haptic_effects[3].description.rate.frequency = r4;
        DumpToConsole(hapticEffect);
    }
    public void SubmitSmellEffect()
    {
        // parse to integer - start
        int.TryParse(start1.text, out int s1); smellEffect.smell_effects[0].start = s1;
        int.TryParse(start2.text, out int s2); smellEffect.smell_effects[1].start = s2;
        int.TryParse(start3.text, out int s3); smellEffect.smell_effects[2].start = s3;
        int.TryParse(start4.text, out int s4); smellEffect.smell_effects[3].start = s4;
        // pattern
        smellEffect.smell_effects[0].description.pattern[0].type = pattern1.text;
        smellEffect.smell_effects[1].description.pattern[0].type = pattern2.text;
        smellEffect.smell_effects[2].description.pattern[0].type = pattern3.text;
        smellEffect.smell_effects[3].description.pattern[0].type = pattern4.text;
        // parse to integer - length-ms
        int.TryParse(length1.text, out int l1); smellEffect.smell_effects[0].description.pattern[0].LengthMs = l1;
        int.TryParse(length2.text, out int l2); smellEffect.smell_effects[1].description.pattern[0].LengthMs = l2;
        int.TryParse(length3.text, out int l3); smellEffect.smell_effects[2].description.pattern[0].LengthMs = l3;
        int.TryParse(length4.text, out int l4); smellEffect.smell_effects[3].description.pattern[0].LengthMs = l4;
        // parse to integer - rate
        int.TryParse(rate1.text, out int r1); smellEffect.smell_effects[0].description.rate.frequency = r1;
        int.TryParse(rate2.text, out int r2); smellEffect.smell_effects[1].description.rate.frequency = r2;
        int.TryParse(rate3.text, out int r3); smellEffect.smell_effects[2].description.rate.frequency = r3;
        int.TryParse(rate4.text, out int r4); smellEffect.smell_effects[3].description.rate.frequency = r4;

        smellEffect.smell_effects[0].description.pattern[0].fragrance = content1.text;
        smellEffect.smell_effects[1].description.pattern[0].fragrance = content2.text;
        smellEffect.smell_effects[2].description.pattern[0].fragrance = content3.text;
        smellEffect.smell_effects[3].description.pattern[0].fragrance = content4.text;
        DumpToConsole(smellEffect);
    }
    public void SubmitTasteEffect()
    {
        // parse to integer - start
        int.TryParse(start1.text, out int s1); tasteEffect.taste_effects[0].start = s1;
        int.TryParse(start2.text, out int s2); tasteEffect.taste_effects[1].start = s2;
        int.TryParse(start3.text, out int s3); tasteEffect.taste_effects[2].start = s3;
        int.TryParse(start4.text, out int s4); tasteEffect.taste_effects[3].start = s4;
        // pattern
        tasteEffect.taste_effects[0].description.pattern[0].type = pattern1.text;
        tasteEffect.taste_effects[1].description.pattern[0].type = pattern2.text;
        tasteEffect.taste_effects[2].description.pattern[0].type = pattern3.text;
        tasteEffect.taste_effects[3].description.pattern[0].type = pattern4.text;
        // parse to integer - length-ms
        int.TryParse(length1.text, out int l1); tasteEffect.taste_effects[0].description.pattern[0].LengthMs = l1;
        int.TryParse(length2.text, out int l2); tasteEffect.taste_effects[1].description.pattern[0].LengthMs = l2;
        int.TryParse(length3.text, out int l3); tasteEffect.taste_effects[2].description.pattern[0].LengthMs = l3;
        int.TryParse(length4.text, out int l4); tasteEffect.taste_effects[3].description.pattern[0].LengthMs = l4;
        // parse to integer - rate
        int.TryParse(rate1.text, out int r1); tasteEffect.taste_effects[0].description.rate.frequency = r1;
        int.TryParse(rate2.text, out int r2); tasteEffect.taste_effects[1].description.rate.frequency = r2;
        int.TryParse(rate3.text, out int r3); tasteEffect.taste_effects[2].description.rate.frequency = r3;
        int.TryParse(rate4.text, out int r4); tasteEffect.taste_effects[3].description.rate.frequency = r4;

        tasteEffect.taste_effects[0].description.pattern[0].flavour = content1.text;
        tasteEffect.taste_effects[1].description.pattern[0].flavour = content2.text;
        tasteEffect.taste_effects[2].description.pattern[0].flavour = content3.text;
        tasteEffect.taste_effects[3].description.pattern[0].flavour = content4.text;
        DumpToConsole(tasteEffect);
    }


}


