using UnityEngine;

namespace Config
{
	public class WindowsGSDescriptor : GSDescriptor
	{
		public WindowsGSDescriptor(TextAsset gsDescriptorJSON) : base(gsDescriptorJSON) { }

		override protected void InitializePlatformValues()
		{
			Contents["store"] = "windows";
			Contents["osVersion"] = "windows";
			Contents["platform"] = "windows";
		}
	}
}