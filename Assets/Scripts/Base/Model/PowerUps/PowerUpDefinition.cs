using UnityEngine;

namespace PowerUps
{
    public class PowerUpDefinition : ScriptableObject, GameObjectDefinition<PowerUpType>
    {
        public PowerUpType Type { get { return type; } }

        public PowerUpType type;
        public BubbleType bubbleType;
        public GameObject prefab;
    }
}
