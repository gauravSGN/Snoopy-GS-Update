using Service;
using UnityEngine;
using System.Collections.Generic;

namespace FTUE
{
    sealed public class TutorialManager : TutorialService
    {
        public TutorialManager()
        {
            var configAsset = GlobalState.AssetService.LoadAsset<TextAsset>("tutorials");
            var config = JsonUtility.FromJson<TutorialConfig>(configAsset.text);
        }
    }
}
