using Service;
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

        public void Click(string nextScene = "")
        {
            if (GlobalState.Instance.Services.Get<UserStateService>().purchasables.hearts.quantity > 0)
            {
                var sceneData = GlobalState.Instance.Services.Get<SceneService>();
                var levelData = GlobalState.Instance.Services.Get<AssetService>().LoadAsset<TextAsset>(levelAssetName);

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

                GlobalState.Instance.Services.Get<PopupService>().Enqueue(new PreLevelPopupConfig
                {
                    level = levelNumber,
                    nextScene = nextScene,
                    stars = GlobalState.Instance.Services.Get<UserStateService>().levels[levelNumber].stars,
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
                    GlobalState.EventService.Dispatch<SnapMapToLocationEvent>(new SnapMapToLocationEvent((RectTransform)transform));
                }
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}