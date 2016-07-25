using UnityEngine;
using System.Collections.Generic;

namespace Service
{
    public interface AssetService : SharedService
    {
        void UnloadUnusedAssets();

        Object LoadAsset(string assetName);
        T LoadAsset<T>(string assetName) where T : Object;
    }
}
