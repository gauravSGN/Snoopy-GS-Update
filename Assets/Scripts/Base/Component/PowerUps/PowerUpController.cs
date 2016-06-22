using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField]
        private PowerUpFactory powerUpFactory;

        [SerializeField]
        private BubbleLauncher launcher;

        private Transform[] anchors;
        private int powerUpMask;
        private Level level;

        void Awake()
        {
            anchors = gameObject.GetComponentsInChildren<Transform>();
            anchors = anchors.Where(child => child != gameObject.transform).ToArray();
        }

        public void Setup(Dictionary<PowerUpType, float> levelData)
        {
            level = gameObject.GetComponentInParent<Level>();
            var index = 0;

            foreach (var data in levelData.Where(data => data.Value > 0.0f))
            {
                if (index == anchors.Count())
                {
                    break;
                }

                var powerUp = powerUpFactory.CreateByType(data.Key);
                powerUp.GetComponent<PowerUp>().Setup((int)(1 / data.Value), this, level);
                powerUp.transform.parent = anchors[index];
                powerUp.transform.localPosition = new Vector3(0, 0);
                index++;
            }
        }

        public void AddPowerUp(PowerUpType type)
        {
            if (powerUpMask == 0)
            {
                launcher.AddShotModifier(AddScan);
            }

            powerUpMask = 1 << (int)type;
        }

        public void AddScan(GameObject bubble)
        {
            bubble.AddComponent<BubbleExplode>();
            bubble.GetComponent<BubbleAttachments>().Model.type = BubbleType.Steel;
            powerUpMask = 0;
        }
    }
}
