using UnityEngine;
using System.Linq;
using Animation;
using Service;

namespace PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField]
        private PowerUpFactory powerUpFactory;

        [SerializeField]
        private BubbleLauncher launcher;

        [SerializeField]
        private AnimationType shooterType;

        [SerializeField]
        private AnimationType effectType;

        [SerializeField]
        private PowerUpScanMap scanMap;

        [SerializeField]
        private BubbleDefinition shooterDefinition;

        private Transform[] anchors;
        private int powerUpMask;
        private AnimationService animationService;

        void Awake()
        {
            var transforms = gameObject.GetComponentsInChildren<Transform>();
            anchors = transforms.Where(child => child != gameObject.transform).ToArray();
        }

        public void Setup(float[] fillData)
        {
            animationService = GlobalState.Instance.Services.Get<AnimationService>();
            scanMap.Load();
            var level = gameObject.GetComponentInParent<Level>();
            var anchorLength = anchors.Length;
            var anchorIndex = 0;

            for (int index = 0, length = fillData.Length; index < length; index++)
            {
                if (anchorIndex < anchorLength)
                {
                    if (fillData[index] > 0.0f)
                    {
                        var powerUp = powerUpFactory.CreateByType((PowerUpType)(1 << index));
                        powerUp.GetComponent<PowerUp>().Setup(fillData[index], this, level);
                        powerUp.transform.parent = anchors[anchorIndex];
                        powerUp.transform.localPosition = Vector3.zero;
                        anchorIndex++;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void AddPowerUp(PowerUpType type)
        {
            if (powerUpMask == 0)
            {
                launcher.AddShotModifier(AddScan);
            }

            launcher.SetModifierAnimation(animationService.CreateByType(shooterType));
            powerUpMask |= (int)type;
        }

        public void AddScan(GameObject bubble)
        {
            // Make bubble unmatchable
            var model = bubble.GetComponent<BubbleAttachments>().Model;
            model.type = BubbleType.Colorless;
            model.definition = shooterDefinition;

            var explosion = bubble.AddComponent<BubbleExplode>();
            explosion.Setup(scanMap.Map[(PowerUpType)powerUpMask], effectType);

            powerUpMask = 0;
        }

        // Just set objects active/inactive until we have an animation
        public void HidePowerUps()
        {
            for (int i = 0, count = anchors.Length; i < count; ++i)
            {
                anchors[i].gameObject.SetActive(false);
            }
        }

        public void ShowPowerUps()
        {
            for (int i = 0, count = anchors.Length; i < count; ++i)
            {
                anchors[i].gameObject.SetActive(true);
            }
        }
    }
}
