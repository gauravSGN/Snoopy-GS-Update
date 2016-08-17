using Util;
using Service;
using Animation;
using UnityEngine;
using System.Linq;
using ScanFunction = Util.CastingUtil.ScanFunction;

namespace PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        private const int THREE_COMBO_ROWS = 3;
        private const int FOUR_COMBO_ROWS = 5;

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

        [SerializeField]
        private AimLine aimline;

        private Transform[] anchors;
        private PowerUpType powerUpType;
        private int totalPowerUpsInUse;
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
                        powerUp.transform.SetParent(anchors[anchorIndex]);
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
            if (powerUpType == PowerUpType.Empty)
            {
                launcher.AddShotModifier(AddScan, ShotModifierType.PowerUp);
                aimline.ModifyAimline(GlobalState.Instance.Config.aimline.extended);
            }

            launcher.SetModifierAnimation(animationService.CreateByType(shooterType));

            powerUpType |= type;
            totalPowerUpsInUse += 1;
        }

        public void AddScan(GameObject bubble)
        {
            // Make bubble unmatchable
            var model = bubble.GetComponent<BubbleAttachments>().Model;
            model.type = BubbleType.Colorless;
            model.definition = shooterDefinition;

            var explosion = bubble.AddComponent<BubbleExplode>();
            explosion.Setup(GetScanFunction(bubble), effectType);

            powerUpType = 0;
            totalPowerUpsInUse = 0;
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

        private ScanFunction GetScanFunction(GameObject bubble)
        {
            ScanFunction scanFunction;

            if (totalPowerUpsInUse == 3)
            {
                scanFunction = () =>
                {
                    return CastingUtil.FullRowBubbleCast(bubble, THREE_COMBO_ROWS, THREE_COMBO_ROWS);
                };
            }
            else if (totalPowerUpsInUse >= 4)
            {
                scanFunction = () =>
                {
                    return CastingUtil.FullRowBubbleCast(bubble, FOUR_COMBO_ROWS, FOUR_COMBO_ROWS);
                };
            }
            else
            {
                var type = powerUpType;
                scanFunction = () => { return CastingUtil.RelativeBubbleCast(bubble, scanMap.Map[type]); };
            }

            return scanFunction;
        }
    }
}
