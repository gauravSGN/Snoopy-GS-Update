using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Scan
{
    [Serializable]
    public sealed class ScanDefinition : IEnumerable<Vector2>
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private Vector2[] locations;

        public IEnumerator<Vector2> GetEnumerator()
        {
            foreach (var location in locations)
            {
                yield return location;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) this.GetEnumerator();
        }
    }
}
