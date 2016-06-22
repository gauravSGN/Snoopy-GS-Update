using UnityEngine;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private GameObject glow;

        [SerializeField]
        private Level level;

        [SerializeField]
        private int max;

        [SerializeField]
        private int current;

        [SerializeField]
        private float progress;

        [SerializeField]
        private int lastBubbleCount;

        private PowerUpDefinition definition;
        private PowerUpController controller;

        public void Setup(int setMax, PowerUpController setController, Level setLevel)
        {
            max = setMax;
            controller = setController;
            level = setLevel;
            level.levelState.AddListener(UpdateState);
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
            if (progress >= 1.0f)
            {
                controller.AddPowerUp(definition.Type);
                Reset();
            }
        }

        private void UpdateState(Observable levelState)
        {
            if ((current < max) && (glow != null))
            {
                var currentBubbleCount = (levelState as LevelState).typeTotals[definition.BubbleType];

                if (currentBubbleCount < lastBubbleCount)
                {
                    var fillRate = ((float)(max - current) - progress) / currentBubbleCount;
                    progress = progress + ((lastBubbleCount - currentBubbleCount) * fillRate);
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
            current++;
        }
    }
}
