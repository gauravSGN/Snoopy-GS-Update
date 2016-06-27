using UnityEngine;

namespace Config
{
    public class iOSGSDescriptor : GSDescriptor
    {
        public iOSGSDescriptor(TextAsset gsDescriptorJSON) : base(gsDescriptorJSON) {}

        override protected void InitializePlatformValues()
        {
            Contents["store"] = "ios";
            Contents["osVersion"] = "iPhone";
            Contents["platform"] = "mobile";
        }
    }
}