
using Core;
using Model.Scan;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PowerUps
{
    public class PowerUpScanMap : ScanMap<PowerUpType>
    {
        [SerializeField]
        private List<TypeAssetPair> scans;

        [Serializable]
        private struct TypeAssetPair
        {
            public PowerUpType type;
            public TextAsset asset;
        }

        override protected List<Tuple<PowerUpType, TextAsset>> GetScans()
        {
            return scans.Select(scan => new Tuple<PowerUpType, TextAsset>(scan.type, scan.asset)).ToList();
        }
    }
}
