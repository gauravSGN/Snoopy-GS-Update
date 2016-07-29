using Model;
using UnityEngine;

namespace UI.Map
{
    public class MapChunkDefinition : ScriptableObject, GameObjectDefinition<MapChunkType>
    {
        public MapChunkType Type { get { return type; } }
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        private MapChunkType type;

        [SerializeField]
        private GameObject prefab;
    }
}
