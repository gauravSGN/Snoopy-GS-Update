using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        private const float FULL_SILHOUETTE = 1.0f;

        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject glow;

        [SerializeField]
        private GameObject filledBackground;

        [SerializeField]
        private GameObject filledIcon;

        [SerializeField]
        private Image fillImage;

        [SerializeField]
        private GameObject fillLine;

        [SerializeField]
        private Animator characterAnimator;

        [SerializeField]
        private float secondsToFill;

        [SerializeField]
        private float max;

        [SerializeField]
        private float current;

        [SerializeField]
        private float progress;

        [SerializeField]
        private int lastBubbleCount;

        [SerializeField]
        private AudioSource filledSound;

        private float currentFillTime;

        private Level level;

        private PowerUpDefinition definition;
        private PowerUpController controller;

        public void Setup(float setMax, PowerUpController setController, Level setLevel, GameObject character)
        {
            max = setMax;
            controller = setController;
            level = setLevel;
            level.levelState.AddListener(UpdateState);
            characterAnimator = character.GetComponent<Animator>();

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

        public void AddPowerUp()
        {
            if (button.interactable && (progress >= 1.0f))
            {
                if (definition.LaunchSound != null)
                {
                    controller.OverrideLaunchSound(definition.LaunchSound);
                }
                controller.AddPowerUp(definition.Type);
                characterAnimator.SetTrigger("AddPowerUp");
                GlobalState.EventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubble);
                Reset();
            }
        }

        private void OnReadyForNextBubble(ReadyForNextBubbleEvent gameEvent)
        {
            characterAnimator.SetTrigger("Finish");
            GlobalState.EventService.RemoveEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubble);
        }

        private void OnInputToggle(InputToggleEvent gameEvent)
        {
            button.interactable = gameEvent.enabled;
        }

        private void OnAddShotModifier(AddShotModifierEvent gameEvent)
        {
            button.interactable = (gameEvent.type == ShotModifierType.PowerUp);
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
                    StartCoroutine(UpdateFillImage());
                }

                lastBubbleCount = currentBubbleCount;

                if (!glow.activeSelf && (progress >= 1.0f))
                {
                    glow.SetActive(true);
                    filledBackground.SetActive(false);
                    filledSound.Play();
                }
            }
        }

        private void Reset()
        {
            filledIcon.SetActive(false);
            glow.SetActive(false);
            filledBackground.SetActive(true);

            var fillLineTransform = (RectTransform)fillLine.transform;
            fillLineTransform.localPosition = new Vector3(fillLineTransform.localPosition.x, 0);

            fillImage.fillAmount = FULL_SILHOUETTE;
            progress = 0.0f;
            current += 1.0f;
        }

        private IEnumerator UpdateFillImage()
        {
            if (currentFillTime <= 0.01f)
            {
                var fillLineTransform = (RectTransform)fillLine.transform;

                filledIcon.SetActive(true);

                while (currentFillTime < secondsToFill)
                {
                    currentFillTime += Time.deltaTime;
                    fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount,
                                                      (FULL_SILHOUETTE - progress), (currentFillTime / secondsToFill));

                    var newY = (FULL_SILHOUETTE - fillImage.fillAmount) * fillLineTransform.rect.height;
                    fillLineTransform.localPosition = new Vector3(fillLineTransform.localPosition.x, newY);

                    yield return null;
                }

                currentFillTime = 0.0f;

                if (progress < 1.0f)
                {
                    filledIcon.SetActive(false);
                }
            }
        }
    }
}
