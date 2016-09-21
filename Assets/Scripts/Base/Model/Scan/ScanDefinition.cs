using System;
using UnityEngine;

namespace Model.Scan
{
    [Serializable]
    public sealed class ScanDefinition
    {
        public string name;
        public Vector2[][] locations;
    }
}
