// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
[Serializable]
public class Description
{
    public List<Pattern> pattern;
    public Rate rate;
}

[Serializable]
public class HapticEffect
{
    public int start;
    public Description description;
}

[Serializable]
public class Pattern
{
    public string type;

    [JsonProperty("length-ms")] public int LengthMs;
}

[Serializable]
public class Rate
{
    public int frequency;
}

[Serializable]
public class Haptic
{
    public List<HapticEffect> haptic_effects;
    public string deviceId;
}
