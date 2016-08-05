using Core;
using UnityEngine;
using System.Collections.Generic;

namespace Model.Scan
{
    abstract public class ScanMap<BaseType> : ScriptableObject
    {
        private Dictionary<BaseType, ScanDefinition> map;

        public Dictionary<BaseType, ScanDefinition> Map
        {
            get
            {
                Load();
                return map;
            }
        }

        abstract protected List<Tuple<BaseType, TextAsset>> GetScans();

        public void Load()
        {
            if (map == null)
            {
                map = new Dictionary<BaseType, ScanDefinition>();

                foreach (var scan in GetScans())
                {
                    var json = scan.Item2.text;
                    map[scan.Item1] = JsonUtility.FromJson<Model.Scan.ScanDefinition>(json);
                }
            }
        }
    }
}
