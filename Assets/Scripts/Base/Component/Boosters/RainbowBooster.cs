using Reaction;
using UnityEngine;
using UI.Callbacks;
using UnityEngine.UI;

namespace Booster
{
    public class RainbowBooster : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlay;

        [SerializeField]
        private BubbleLauncher launcher;

        [SerializeField]
        private Level level;

        [SerializeField]
        private BubbleDefinition shooterDefinition;

        [SerializeField]
        private Button boosterButton;

        [SerializeField]
        private PurchaseBoosters purchaseBoosters;

        [SerializeField]
        private AudioSource equipSound;

        private GameObject instantiatedOverlay;

        public void ActivateOrBuy()
        {
            if (GlobalState.User.purchasables.boosters.rainbows == 0)
            {
                purchaseBoosters.BuyRainbows();
            }
            else if ((instantiatedOverlay == null) &&
                     (level.levelState.remainingBubbles >= 2))
            {
                instantiatedOverlay = Instantiate(overlay);

                launcher.AddShotModifier(ConvertToRainbow, ShotModifierType.RainbowBooster);
                launcher.SetModifierAnimation(instantiatedOverlay);

                GlobalState.User.purchasables.boosters.rainbows--;
                level.levelState.DecrementRemainingBubbles();

                equipSound.Play();
            }
        }

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<AddShotModifierEvent>(OnAddShotModifier);
        }

        private void OnAddShotModifier(AddShotModifierEvent gameEvent)
        {
            boosterButton.interactable = false;
        }

        private void ConvertToRainbow(GameObject bubble)
        {
            var model = bubble.GetComponent<BubbleAttachments>().Model;

            model.type = BubbleType.Rainbow;
            model.definition = shooterDefinition;

            BubbleReactionEvent.Dispatch(ReactionPriority.CullRainbow, model);

            bubble.GetComponent<BubbleDeath>().DeactivateObjectOnDeath(instantiatedOverlay);
            instantiatedOverlay = null;
        }
    }
}
