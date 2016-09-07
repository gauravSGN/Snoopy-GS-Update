using UnityEngine;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface AssetService : SharedService
    {
        event Action<float> OnProgress;
        event Action OnComplete;

        float Progress { get; }
        bool IsLoading { get; }

        void UnloadUnusedAssets();

        T LoadAsset<T>(string assetName) where T : UnityEngine.Object;

        void LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object;
        void LoadAssetAsync<T>(string assetName, Action<T> callback) where T : UnityEngine.Object;
    }
}
