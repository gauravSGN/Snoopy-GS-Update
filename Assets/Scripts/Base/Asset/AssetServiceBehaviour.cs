using UnityEngine;
using System.Collections.Generic;
using Service;

namespace Asset
{
    public class AssetServiceBehaviour : MonoBehaviour, AssetService
    {
        [SerializeField]
        private List<AssetGroup> assetGroups;

        private readonly Dictionary<string, string> assets = new Dictionary<string, string>();

        public void Start()
        {
            PopulateAssetTable();

            GlobalState.Instance.Services.SetInstance<AssetService>(this);
        }

        public void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }

        public Object LoadAsset(string assetName)
        {
            return Resources.Load(ResolveAssetName(assetName));
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            return Resources.Load(ResolveAssetName(assetName), typeof(T)) as T;
        }

        private string ResolveAssetName(string name)
        {
            var result = name;

            if (assets.ContainsKey(name))
            {
                result = assets[name];
            }

            return result;
        }

        private void PopulateAssetTable()
        {
            var platform = ApplicationPlatform.Android;

            foreach (var asset in assetGroups)
            {
                foreach (var substitution in asset.Substitutions)
                {
                    if (substitution.Platform == platform)
                    {
                        assets.Add(asset.AssetName, substitution.AssetName);
                    }
                }
            }
        }
    }
}
