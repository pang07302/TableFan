
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace SmellName
{
    [Serializable]
    public class Description
    {
        public List<Pattern> pattern;
        public Rate rate;
    }

    [Serializable]
    public class SmellEffect
    {
        public int start;
        public Description description;
    }

    [Serializable]
    public class Pattern
    {
        public string type;

        [JsonProperty("length-ms")] public int LengthMs;
        public string fragrance;
    }

    [Serializable]
    public class Rate
    {
        public int frequency;
    }

    [Serializable]
    public class Smell
    {
        public List<SmellEffect> smell_effects;
        public string deviceId;
        public string control;
    }
}