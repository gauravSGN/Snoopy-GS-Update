using Service;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

public class MapSceneTransition : MonoBehaviour
{
    [SerializeField]
    private string levelAssetName;

    [SerializeField]
    private int levelNumber;

    [SerializeField]
    private Text buttonText;

    [SerializeField]
    private GameObject[] starPositions;

    public void Initiate(string nextScene = "")
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

        var user = GlobalState.Instance.Services.Get<UserStateService>();

        for (long starIndex = 0, filledStars = user.levels[levelNumber].stars; starIndex < filledStars; ++starIndex)
        {
            starPositions[starIndex].GetComponent<Image>().enabled = true;
        }
    }
}
