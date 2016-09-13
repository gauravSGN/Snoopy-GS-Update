using Sound;
using System;
using Effects;
using UI.Popup;
using Animation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    public class MapButton : MonoBehaviour
    {
        static private readonly string RESOURCE_PREFIX = "Levels/Level_";
        static private readonly SoundType[] starSounds = new [] {
            SoundType.WinStars1,
            SoundType.WinStars2,
            SoundType.WinStars3
        };

        [SerializeField]
        private string levelAssetName;

        [SerializeField]
        private int levelNumber;

        [SerializeField]
        private Selectable button;

        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private Image[] starPositions;

        [SerializeField]
        private Image buttonIcon;

        private long filledStars;

        public void Click(string nextScene)
        {
            if (!GlobalState.Instance.Services.Get<Service.BlockadeService>().InputBlocked &&
                (GlobalState.User.purchasables.hearts.quantity > 0))
            {
                var sceneData = GlobalState.SceneService;
                var levelData = GlobalState.AssetService.LoadAsset<TextAsset>(levelAssetName);

                if (levelData != null)
                {
                    sceneData.NextLevelData = levelData.text;
                }

                sceneData.LevelNumber = levelNumber;
                sceneData.ReturnScene = StringConstants.Scenes.MAP;

                if (nextScene == "")
                {
                    nextScene = StringConstants.Scenes.LEVEL;
                }

                GlobalState.PopupService.Enqueue(new PreLevelPopupConfig
                {
                    level = levelNumber,
                    nextScene = nextScene,
                    stars = GlobalState.User.levels[levelNumber].stars,
                });
            }
        }

        protected void Awake()
        {
            if (levelNumber == 0)
            {
                var parentName = gameObject.transform.parent.name;
                levelNumber = Convert.ToInt32(parentName);
                levelAssetName = RESOURCE_PREFIX + parentName;
            }
        }

        protected void Start()
        {
            buttonText.text = levelNumber.ToString();

            var user = GlobalState.User;

            if (user.maxLevel >= levelNumber)
            {
                filledStars = user.levels[levelNumber].stars;

                for (long starIndex = 0; starIndex < filledStars; ++starIndex)
                {
                    starPositions[starIndex].enabled = true;
                }

                if (user.currentLevel == levelNumber)
                {
                    GlobalState.EventService.Dispatch(new SnapMapToLocationEvent((RectTransform)transform));
                    GlobalState.EventService.AddEventHandler<AnimateStarsOnMapNodeEvent>(OnAnimateStarsOnMapNodeEvent);
                }

                if (user.maxLevel == levelNumber)
                {
                    RectTransform previousMapButtonTransform = null;
                    var previousMapButton = transform.parent.parent.Find((levelNumber - 1).ToString());

                    if ((user.levels[levelNumber].score == 0) && previousMapButton)
                    {
                        previousMapButtonTransform = previousMapButton.transform as RectTransform;
                    }

                    var gameEvent = new SetPlayerAvatarPositionEvent(previousMapButtonTransform,
                                                                     (RectTransform)transform);

                    GlobalState.EventService.Dispatch(gameEvent);
                }
            }
            else
            {
                button.interactable = false;
                buttonIcon.enabled = false;
            }
        }

        private void OnAnimateStarsOnMapNodeEvent(AnimateStarsOnMapNodeEvent gameEvent)
        {
            GlobalState.EventService.RemoveEventHandler<AnimateStarsOnMapNodeEvent>(OnAnimateStarsOnMapNodeEvent);

            for (long starIndex = gameEvent.oldStars; starIndex < filledStars; ++starIndex)
            {
                // We need to make a copy of starIndex so our lambda doesn't reference it directly
                var index = starIndex;
                var newStarIndex = (index - gameEvent.oldStars);
                var delay = (0.4f * (newStarIndex + 1));

                starPositions[index].enabled = false;

                Util.FrameUtil.AfterDelay(delay, () =>
                {
                    PlaySoundEvent.Dispatch(starSounds[newStarIndex]);
                    StartCoroutine(AnimationEffect.Play(starPositions[index].gameObject,
                                                        AnimationType.StarOnMapNode));
                });
            }
        }
    }
}