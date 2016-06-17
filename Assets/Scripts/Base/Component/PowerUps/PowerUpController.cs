using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField]
        private PowerUpFactory powerUpFactory;

        // [SerializeField]
        // private AreaEffectFactory areaEffectFactory;

        private Transform[] anchors;

        void Awake()
        {
            anchors = gameObject.GetComponentsInChildren<Transform>();
        }

        public void Setup(Dictionary<PowerUpType, float> levelData)
        {
            var index = 1;

            foreach (var data in levelData.Where(data => data.Value > 0.0f))
            {
                if (index == anchors.Count())
                {
                    break;
                }

                var powerUp = powerUpFactory.CreatePowerUpByType(data.Key);
                powerUp.GetComponent<PowerUp>().Setup((int)data.Value);
                powerUp.transform.parent = anchors[index];
                powerUp.transform.localPosition = new Vector3(0, 0);
                index++;
            }
        }
    }
}