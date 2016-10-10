using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public static class GSDescriptorFactory
    {
        private static Dictionary<RuntimePlatform, Type> platformClassMap = new Dictionary<RuntimePlatform, Type>
        {
            {RuntimePlatform.Android, typeof(AndroidGSDescriptor)},
            {RuntimePlatform.IPhonePlayer, typeof(iOSGSDescriptor)},
			{RuntimePlatform.WindowsPlayer, typeof(WindowsGSDescriptor)}
        };

        public static GSDescriptor CreateByPlatform(RuntimePlatform platform, TextAsset source)
        {
			// We need to override our platform if we are using the editor
#if UNITY_EDITOR && UNITY_ANDROID
                platform = RuntimePlatform.Android;
#elif UNITY_EDITOR && UNITY_IOS
                platform = RuntimePlatform.IPhonePlayer;
#elif UNITY_EDITOR && UNITY_STANDALONE_WIN
				platform = RuntimePlatform.WindowsPlayer;
#elif UNITY_STANDALONE_WIN
				platform = RuntimePlatform.WindowsPlayer;
#endif
			return (GSDescriptor)Activator.CreateInstance(platformClassMap[platform], new System.Object[] { source });
        }
    }
}
