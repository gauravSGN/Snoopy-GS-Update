using UnityEngine;

namespace BubbleContent
{
    public sealed class BubbleContentDefinition : ScriptableObject, GameObjectDefinition<BubbleContentType>
    {
        public BubbleContentType Type { get { return type; } }

        public BubbleContentType type;
        public GameObject prefab;
    }
}
