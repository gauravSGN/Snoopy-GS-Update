using UnityEngine;

namespace Model
{
    public interface GameObjectDefinition<out DefinitionEnumType>
    {
        DefinitionEnumType Type { get; }
        GameObject Prefab { get; }
    }
}
