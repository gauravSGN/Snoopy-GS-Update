using UnityEngine;
using System.Collections.Generic;

namespace Util
{
    public class ScriptableList<ItemType> : ScriptableObject
    {
        [SerializeField]
        protected List<ItemType> items;

        public IEnumerable<ItemType> Items { get { return items; } }
    }
}
