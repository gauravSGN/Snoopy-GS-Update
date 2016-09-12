using System;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    public class MapButton : MonoBehaviour
    {
        private const string RESOURCE_PREFIX = "Levels/Level_";

        [SerializeField]
        private string levelAssetName;

        [SerializeField]
        private int levelNumber;

        [SerializeField]
        private Selectable button;

        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private GameObject[] starPositions;

        [SerializeField]
        private Image buttonIcon;

        public void Click(string nextScene)
        {
            if (GlobalState.User.purchasables.hearts.quantity > 0)
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
                var filledStars = user.levels[levelNumber].stars;

                for (long starIndex = 0; starIndex < filledStars; ++starIndex)
                {
                    starPositions[starIndex].GetComponent<Image>().enabled = true;
                }

                if (user.currentLevel == levelNumber)
                {
                    GlobalState.EventService.Dispatch(new SnapMapToLocationEvent((RectTransform)transform));
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
    }
}