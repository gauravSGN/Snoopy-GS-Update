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
            var instance = Instantiate(definition.Prefab);
            instance.GetComponent<PowerUp>().SetDefinition(definition);
            return instance;
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
