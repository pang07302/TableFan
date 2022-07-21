
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace SightName
{
    [Serializable]
    public class Description
    {
        public List<Pattern> pattern;
        public Rate rate;
    }

    [Serializable]
    public class SightEffect
    {
        public int start;
        public Description description;
    }

    [Serializable]
    public class Pattern
    {
        public string type;

        [JsonProperty("length-ms")] public int LengthMs;
        public string colour;
    }

    [Serializable]
    public class Rate
    {
        public int frequency;
    }

    [Serializable]
    public class Sight
    {
        public List<SightEffect> sight_effects;
        public string deviceId;
    }
}