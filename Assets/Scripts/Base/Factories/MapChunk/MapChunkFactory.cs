using Util;
using UnityEngine;

namespace UI.Map
{
    public class MapChunkFactory : ScriptableFactory<MapChunkType, MapChunkDefinition>
    {
        override public GameObject CreateByType(MapChunkType type)
        {
            var definition = GetDefinitionByType(type);
            return Instantiate(definition.Prefab);
        }
    }
}
