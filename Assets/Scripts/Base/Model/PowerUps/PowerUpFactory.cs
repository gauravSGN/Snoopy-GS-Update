using UnityEngine;
using System.Collections.Generic;

namespace PowerUps
{
    public class PowerUpFactory : ScriptableObject
    {
        [SerializeField]
        private List<PowerUpDefinition> powerUps;

        [SerializeField]
        private Dictionary<PowerUpType, PowerUpDefinition> powerUpLookUp;


        public GameObject CreatePowerUpByType(PowerUpType type)
        {
            var definition = GetBubbleDefinitionByType(type);
            return Instantiate(definition.prefab);
        }

        private PowerUpDefinition GetBubbleDefinitionByType(PowerUpType type)
        {
            powerUpLookUp = powerUpLookUp ?? CreateLookupTable<PowerUpType, PowerUpDefinition>(powerUps);

            return powerUpLookUp[type];
        }

        private Dictionary<K, V> CreateLookupTable<K, V>(List<V> items) where V : GameObjectDefinition<K>
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
