using UnityEngine;
using System.Collections.Generic;
using Model;

namespace Util
{
    abstract public class ScriptableFactory<BaseType, DefinitionType> : ScriptableObject
        where DefinitionType : GameObjectDefinition<BaseType>
    {
        [SerializeField]
        protected List<DefinitionType> definitions;

        protected Dictionary<BaseType, DefinitionType> definitionLookup;

        virtual public GameObject CreateByType(BaseType type)
        {
            var definition = GetDefinitionByType(type);
            return Instantiate(definition.Prefab);
        }

        virtual protected DefinitionType GetDefinitionByType(BaseType type)
        {
            definitionLookup = definitionLookup ?? CreateLookupTable<BaseType, DefinitionType>(definitions);

            return definitionLookup[type];
        }
        protected Dictionary<K, V> CreateLookupTable<K, V>(List<V> items) where V : GameObjectDefinition<K>
        {
            var lookup = new Dictionary<K, V>();

            foreach (var info in items)
            {
                lookup.Add(info.Type, info);
            }

            return lookup;
        }
    }
}