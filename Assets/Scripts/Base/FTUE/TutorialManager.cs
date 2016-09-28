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

        private readonly List<TutorialConfig.TutorialData> tutorials;
        private readonly Dictionary<string, TutorialConfig.RewardList> rewards;
        private readonly TutorialRewardFactory rewardFactory = new TutorialRewardFactory();

        public TutorialManager()
        {
            var configAsset = GlobalState.AssetService.LoadAsset<TextAsset>(CONFIG_PATH);
            var config = JsonUtility.FromJson<TutorialConfig>(configAsset.text);

            tutorials = config.tutorials;
            rewards = config.rewardLists.ToDictionary(rl => rl.id);

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
                   !GlobalState.User.tutorials.HasCompleted(tutorial.id);
        }

        private void GiveRewards(string rewardID)
        {
            TutorialConfig.RewardList rewardList;

            if (rewards.TryGetValue(rewardID, out rewardList))
            {
                foreach (var reward in rewardList.rewards)
                {
                    rewardFactory.Create(reward.item).Apply(reward.count);
                }
            }
        }

        private void ShowTutorial(TutorialConfig.TutorialData tutorial)
        {
            // Temporarily disabled for evaluation and feedback
            //GlobalState.User.tutorials.MarkCompleted(tutorial.id);

            GlobalState.EventService.Dispatch(new ShowTutorialEvent(tutorial.id));
        }
    }
}
