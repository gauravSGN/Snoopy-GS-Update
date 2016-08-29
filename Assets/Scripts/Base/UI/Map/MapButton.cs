using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    public class MapButton : MonoBehaviour
    {
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
                    var previousMapButton = transform.parent.Find((levelNumber - 1).ToString());
                    var previousMapButtonTransform = previousMapButton ? previousMapButton.transform : null;

                    var gameEvent = new SetPlayerAvatarPositionEvent((RectTransform)previousMapButtonTransform,
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