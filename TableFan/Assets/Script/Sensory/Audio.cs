
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace AudioName
{
    [Serializable]
    public class Description
    {
        public List<Pattern> pattern;
        public Rate rate;
    }

    [Serializable]
    public class AudioEffect
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
    public class Audio
    {
        public List<AudioEffect> audio_effects;
        public string deviceId;
    }
}