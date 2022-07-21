
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace TasteName
{
    [Serializable]
    public class Description
    {
        public List<Pattern> pattern;
        public Rate rate;
    }

    [Serializable]
    public class TasteEffect
    {
        public int start;
        public Description description;
    }

    [Serializable]
    public class Pattern
    {
        public string type;

        [JsonProperty("length-ms")] public int LengthMs;
        public string flavour;
    }

    [Serializable]
    public class Rate
    {
        public int frequency;
    }

    [Serializable]
    public class Taste
    {
        public List<TasteEffect> taste_effects;
        public string deviceId;
        public string control;
    }
}