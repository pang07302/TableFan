using System;
namespace TasteName
{
    [Serializable]
    public class Description
    {
        public Properties properties;
        public Pattern pattern;
    }
    [Serializable]
    public class Pattern
    {
        public string type;
        public int LengthMs;
    }
    [Serializable]
    public class Properties
    {
        public string type;
        public string measure;
        public string unit;
        public int quantity;
    }
    [Serializable]
    public class Taste
    {
        public string deviceId;
        public string control;
        public string category;
        public Description description;
    }
}