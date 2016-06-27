using UnityEngine;

namespace Config
{
    public class AndroidGSDescriptor : GSDescriptor
    {
        public AndroidGSDescriptor(TextAsset gsDescriptorJSON) : base(gsDescriptorJSON) {}

        override protected void InitializePlatformValues()
        {
            Contents["store"] = "android";
            Contents["osVersion"] = "Android";
            Contents["platform"] = "mobile";
        }
    }
}