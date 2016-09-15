using Service;
using UnityEngine;
using System.Linq;
using FTUE.Rewards;
using System.Collections.Generic;

namespace FTUE
{
    sealed public class TutorialManager : TutorialService
    {
        private const string CONFIG_PATH = "Tutorials/config";

        private List<TutorialConfig.TutorialData> tutorials;
        private Dictionary<string, TutorialConfig.RewardList> rewards;
        private readonly TutorialRewardFactory rewardFactory = new TutorialRewardFactory();
        private UserStateService user;

        public TutorialManager()
        {
            var configAsset = GlobalState.AssetService.LoadAsset<TextAsset>(CONFIG_PATH);
            var config = JsonUtility.FromJson<TutorialConfig>(configAsset.text);

            tutorials = config.tutorials;
            rewards = config.rewardLists.ToDictionary(rl => rl.id);
            user = GlobalState.User;

            GlobalState.EventService.Persistent.AddEventHandler<TutorialProgressEvent>(OnTutorialProgress);
        }

        private void OnTutorialProgress(TutorialProgressEvent gameEvent)
        {
            foreach (var tutorial in tutorials)
            {
                if (IsTutorialApplicable(tutorial, gameEvent))
                {
                    GiveRewards(tutorial.reward);
                    ShowTutorial(tutorial);
                }
            }
        }

        private bool IsTutorialApplicable(TutorialConfig.TutorialData tutorial, TutorialProgressEvent progress)
        {
            return (tutorial.trigger == progress.trigger) &&
                   (tutorial.level == progress.level) &&
                   !user.tutorials.HasCompleted(tutorial.id);
        }

        private void GiveRewards(string rewardID)
        {
            TutorialConfig.RewardList rewardList;

            if (rewards.TryGetValue(rewardID, out rewardList))
            {
                foreach (var reward in rewardList.rewards)
                {
                    Debug.Log(string.Format("TutorialManager: Giving {0} of {1} as reward", reward.count, reward.item));
                    rewardFactory.Create(reward.item).Apply(reward.count);
                }
            }
        }

        private void ShowTutorial(TutorialConfig.TutorialData tutorial)
        {
            user.tutorials.MarkCompleted(tutorial.id);

            GlobalState.EventService.Dispatch(new ShowTutorialEvent(tutorial.id));
        }
    }
}
