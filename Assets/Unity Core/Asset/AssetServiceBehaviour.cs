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

        public float Progress { get; private set; }
        public bool IsLoading { get; private set; }

        private interface AsyncAssetRequest
        {
            ResourceRequest Request { get; }
            UnityEngine.Object Asset { get; set; }

            void InvokeCallback();
        }

        private class AsyncAssetRequest<T> : AsyncAssetRequest where T : UnityEngine.Object
        {
            public ResourceRequest Request { get; set; }
            public UnityEngine.Object Asset { get; set; }
            public Action<T> Callback { get; set; }

            public void InvokeCallback()
            {
                if (Callback != null)
                {
                    Callback.Invoke(Asset as T);
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
                Request = Resources.LoadAsync<T>(assetName),
                Callback = callback,
            });

            if (requests.Count == 1)
            {
                StartCoroutine(AsyncLoadRoutine());
            }
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
            IsLoading = true;
            int numComplete = 0;

            while (requests.Count > 0)
            {
                int index = 0;
                float progress = 0.0f;

                while (index < requests.Count)
                {
                    var request = requests[index];

                    if (request.Request.isDone)
                    {
                        request.Asset = request.Request.asset;
                        request.InvokeCallback();

                        numComplete++;
                        requests.RemoveAt(index);
                    }
                    else
                    {
                        progress += request.Request.progress;
                        index++;
                    }
                }

                Progress = (progress + numComplete) / (float)(requests.Count + numComplete);

                if (OnProgress != null)
                {
                    OnProgress.Invoke(Progress);
                }

                yield return null;
            }

            IsLoading = false;

            if (OnComplete != null)
            {
                OnComplete.Invoke();
            }
        }
    }
}
