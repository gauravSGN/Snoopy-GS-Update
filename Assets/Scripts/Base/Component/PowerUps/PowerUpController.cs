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

        [SerializeField]
        private AnimatorOverrideController shooterAnimation;

        [SerializeField]
        private AnimatorOverrideController effectAnimation;

        [SerializeField]
        private Animator animationPrefab;

        private Transform[] anchors;
        private int powerUpMask;
        private Level level;

        void Awake()
        {
            var transforms = gameObject.GetComponentsInChildren<Transform>();
            anchors = transforms.Where(child => child != gameObject.transform).ToArray();
        }

        public void Setup(Dictionary<PowerUpType, float> levelData)
        {
            level = gameObject.GetComponentInParent<Level>();
            var index = 0;
            var length = anchors.Length;

            foreach (var data in levelData.Where(data => data.Value > 0.0f))
            {
                if (index == length)
                {
                    break;
                }

                var powerUp = powerUpFactory.CreateByType(data.Key);
                powerUp.GetComponent<PowerUp>().Setup((int)(1 / data.Value), this, level);
                powerUp.transform.parent = anchors[index];
                powerUp.transform.localPosition = Vector3.zero;
                index++;
            }
        }

        public void AddPowerUp(PowerUpType type)
        {
            if (powerUpMask == 0)
            {
                launcher.AddShotModifier(AddScan);
            }

            Animator animation = (Animator) Instantiate(animationPrefab, Vector3.zero, Quaternion.identity);
            animation.runtimeAnimatorController = shooterAnimation;
            launcher.SetModifierAnimation(animation.gameObject);
            powerUpMask |= (int)type;
        }

        public void AddScan(GameObject bubble)
        {
            // Make bubble unmatchable
            bubble.GetComponent<BubbleAttachments>().Model.type = BubbleType.Colorless;

            bubble.AddComponent<BubbleExplode>();
            bubble.GetComponent<BubbleExplode>().Setup(2, effectAnimation);
            powerUpMask = 0;
        }
    }
}
