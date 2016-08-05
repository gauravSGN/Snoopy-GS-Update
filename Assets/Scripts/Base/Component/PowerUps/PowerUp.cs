using UnityEngine;
using Service;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private GameObject glow;

        [SerializeField]
        private Level level;

        [SerializeField]
        private float max;

        [SerializeField]
        private float current;

        [SerializeField]
        private float progress;

        [SerializeField]
        private int lastBubbleCount;

        private bool allowInput;

        private PowerUpDefinition definition;
        private PowerUpController controller;

        public void Setup(float setMax, PowerUpController setController, Level setLevel)
        {
            max = setMax;
            controller = setController;
            level = setLevel;
            level.levelState.AddListener(UpdateState);

            GlobalState.EventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
            GlobalState.EventService.AddEventHandler<AddShotModifierEvent>(OnAddShotModifier);
        }

        public void SetDefinition(PowerUpDefinition setDefinition)
        {
            if (definition == null)
            {
                definition = setDefinition;
            }
        }

        protected void OnMouseUp()
        {
            if (progress >= 1.0f && allowInput)
            {
                controller.AddPowerUp(definition.Type);
                Reset();
            }
        }

        private void OnInputToggle(InputToggleEvent gameEvent)
        {
            allowInput = gameEvent.enabled;
        }

        private void OnAddShotModifier(AddShotModifierEvent gameEvent)
        {
            allowInput = (gameEvent.type == ShotModifierType.PowerUp);
        }

        private void UpdateState(Observable levelState)
        {
            if ((current < max) && (glow != null))
            {
                var currentBubbleCount = (levelState as LevelState).typeTotals[definition.BubbleType];

                if (currentBubbleCount < lastBubbleCount)
                {
                    var fillRate = ((max - current) - progress) / Mathf.Max(1, currentBubbleCount);
                    progress += (lastBubbleCount - currentBubbleCount) * fillRate;
                }

                lastBubbleCount = currentBubbleCount;

                if (!glow.activeSelf && (progress >= 1.0f))
                {
                    glow.SetActive(true);
                }
            }
        }

        private void Reset()
        {
            glow.SetActive(false);
            progress = 0.0f;
            current += 1.0f;
        }
    }
}
