using System.Collections.Generic;

namespace FTUE.Rewards
{
    sealed public class TutorialRewardFactory
    {
        private readonly Dictionary<string, TutorialReward> rewards = new Dictionary<string, TutorialReward>();
        private readonly TutorialReward defaultReward = new NullReward();

        public TutorialReward Create(string itemType)
        {
            TutorialReward reward = defaultReward;

            if (rewards.ContainsKey(itemType))
            {
                reward = rewards[itemType];
            }

            return reward;
        }
    }
}
