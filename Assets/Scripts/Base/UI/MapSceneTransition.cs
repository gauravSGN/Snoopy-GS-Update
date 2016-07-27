﻿using Service;
using UI.Popup;
using UnityEngine;

public class MapSceneTransition : MonoBehaviour
{
    [SerializeField]
    private string levelAssetName;

    [SerializeField]
    private int levelNumber;

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
}
