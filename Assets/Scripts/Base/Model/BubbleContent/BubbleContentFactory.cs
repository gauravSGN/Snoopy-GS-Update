using System.Collections.Generic;
using UnityEngine;
using Util;

namespace BubbleContent
{
    public class BubbleContentFactory : ScriptableFactory<BubbleContentType, BubbleContentDefinition>
    {
        public IEnumerable<BubbleContentDefinition> Contents { get { return definitions; } }

        override public GameObject CreateByType(BubbleContentType type)
        {
            var definition = GetDefinitionByType(type);
            GameObject content = null;

            if (definition != null)
            {
                content = Instantiate(definition.Prefab);
            }

            return content;
        }

        override public BubbleContentDefinition GetDefinitionByType(BubbleContentType type)
        {
            definitionLookup = definitionLookup ?? CreateLookupTable<BubbleContentType, BubbleContentDefinition>(definitions);
            BubbleContentDefinition definition = null;

            definitionLookup.TryGetValue(type, out definition);

            return definition;
        }
    }
}
