using Sequence;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        private const float FULL_SILHOUETTE = 1.0f;
        private const float TRANSITION_TIME = 0.2f;

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
        private AnimationCurve hideCurve = AnimationCurve.Linear(0, 1, 1, 0);

        [SerializeField]
        private AnimationCurve showCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private float currentFillTime;

        private PowerUpDefinition definition;
        private PowerUpController controller;
        private Animator ownAnimator;

        public void Setup(float setMax, PowerUpController setController, Level setLevel, GameObject character)
        {
            max = setMax;
            controller = setController;
            setLevel.levelState.AddListener(UpdateState);
            character.SetActive(true);
            characterAnimator = character.GetComponent<Animator>();
            ownAnimator = GetComponent<Animator>();

            var eventService = GlobalState.EventService;

            eventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
            eventService.AddEventHandler<AddShotModifierEvent>(OnAddShotModifier);
            eventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
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
                Sound.PlaySoundEvent.Dispatch(Sound.SoundType.PowerUpCast);
                GlobalState.EventService.Dispatch<InputToggleEvent>(new InputToggleEvent(false));

                if (definition.LaunchSound != null)
                {
                    controller.OverrideLaunchSound(definition.LaunchSound);
                }

                controller.AddPowerUp(definition.Type);
                GlobalState.EventService.AddEventHandler<PowerUpPrepareForReturnEvent>(OnItemReturn);
                Reset();
                StartCoroutine(ShowCharacter());
            }
        }

        public void Hide()
        {
            StartCoroutine(HideShow(hideCurve));
        }

        public void Show()
        {
            StartCoroutine(HideShow(showCurve));
        }

        private void OnItemReturn()
        {
            characterAnimator.SetTrigger("Finish");
            characterAnimator.ResetTrigger("AddPowerUp");
            Show();
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
                    ownAnimator.SetTrigger("Charged");
                    glow.SetActive(true);
                    filledBackground.SetActive(false);
                    Sound.PlaySoundEvent.Dispatch(Sound.SoundType.PowerUpFill);
                }
            }
        }

        private void Reset()
        {
            ownAnimator.SetTrigger("Fired");
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

        private IEnumerator HideShow(AnimationCurve curve)
        {
            float time = 0f;
            float newValue;

            while (time <= TRANSITION_TIME)
            {
                time += Time.deltaTime;
                newValue = curve.Evaluate(time / TRANSITION_TIME);
                transform.localScale = new Vector3(newValue, newValue, 1);
                yield return null;
            }

            newValue = curve.Evaluate(1f);
            transform.localScale = new Vector3(newValue, newValue, 1);
        }

        private IEnumerator ShowCharacter()
        {
            yield return StartCoroutine(HideShow(hideCurve));
            characterAnimator.SetTrigger("AddPowerUp");
        }

        private void OnPrepareForBubbleParty()
        {
            Hide();
            ownAnimator.SetTrigger("Fired");
        }
    }
}
