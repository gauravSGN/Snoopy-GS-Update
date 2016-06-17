using UnityEngine;

public interface GameObjectDefinition<out DefinitionEnumType>
{
    DefinitionEnumType Type { get; }
    GameObject Prefab { get; }
}
