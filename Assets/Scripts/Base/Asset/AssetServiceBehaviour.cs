using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Service;

namespace Asset
{
    public class AssetServiceBehaviour : MonoBehaviour, AssetService
    {
        public event Action<float> OnProgress;
        public event Action OnComplete;

        abstract private class AsyncAssetRequest
        {
            public ResourceRequest request;
            public UnityEngine.Object asset;

            abstract public void InvokeCallback();
        }

        private class AsyncAssetRequest<T> : AsyncAssetRequest where T : UnityEngine.Object
        {
            public Action<T> callback;

            override public void InvokeCallback()
            {
                if (callback != null)
                {
                    callback.Invoke(asset as T);
                }
            }
        }

        [SerializeField]
        private List<AssetGroup> assetGroups;

        private readonly Dictionary<string, string> assets = new Dictionary<string, string>();
        private readonly List<AsyncAssetRequest> requests = new List<AsyncAssetRequest>();

        public void Start()
        {
            PopulateAssetTable();

            GlobalState.Instance.Services.SetInstance<AssetService>(this);
        }

        public void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }

        public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            return Resources.Load<T>(assetName);
        }

        public void LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            LoadAssetAsync<T>(assetName, null);
        }

        public void LoadAssetAsync<T>(string assetName, Action<T> callback) where T : UnityEngine.Object
        {
            requests.Add(new AsyncAssetRequest<T>
            {
                request = Resources.LoadAsync<T>(assetName),
                callback = callback,
            });

            if (requests.Count == 1)
            {
                StartCoroutine(AsyncLoadRoutine());
            }
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

        private IEnumerator AsyncLoadRoutine()
        {
            int numComplete = 0;

            while (requests.Count > 0)
            {
                int index = 0;
                float progress = 0.0f;

                while (index < requests.Count)
                {
                    var request = requests[index];

                    if (request.request.isDone)
                    {
                        request.asset = request.request.asset;
                        request.InvokeCallback();

                        numComplete++;
                        requests.RemoveAt(index);
                    }
                    else
                    {
                        progress += request.request.progress;
                        index++;
                    }
                }

                if (OnProgress != null)
                {
                    progress = (progress + numComplete) / (float)(requests.Count + numComplete);
                    OnProgress.Invoke(progress);
                }

                yield return null;
            }

            if (OnComplete != null)
            {
                OnComplete.Invoke();
            }
        }
    }
}
