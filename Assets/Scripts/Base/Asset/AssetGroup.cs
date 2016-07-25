using UnityEngine;
using System;
using System.Collections.Generic;

namespace Asset
{
    [Serializable]
    public class AssetGroup
    {
        [Serializable]
        public class Substitute
        {
            [SerializeField]
            private ApplicationPlatform platform;

            [SerializeField]
            private string assetName;

            public ApplicationPlatform Platform { get { return platform; } }
            public string AssetName { get { return assetName; } }
        }

        [SerializeField]
        private string assetName;

        [SerializeField]
        private List<Substitute> substitutions;

        public string AssetName { get { return assetName; } }
        public IEnumerable<Substitute> Substitutions { get { return substitutions; } }
    }
}
