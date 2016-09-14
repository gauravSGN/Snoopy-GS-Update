using System;
using UnityEngine;
using System.Collections.Generic;

namespace FTUE
{
    [Serializable]
    sealed public class TutorialConfig
    {
        [Serializable]
        sealed public class TutorialData
        {
            [SerializeField]
            public string id;

            [SerializeField]
            public TutorialTrigger trigger;

            [SerializeField]
            public int level;

            [SerializeField]
            public bool skippable;

            [SerializeField]
            public string config;

            [SerializeField]
            public string reward;

            [SerializeField]
            public string text;
        }

        [Serializable]
        sealed public class Reward
        {
            [SerializeField]
            public string item;

            [SerializeField]
            public int count;
        }

        [Serializable]
        sealed public class RewardList
        {
            [SerializeField]
            public string id;

            [SerializeField]
            public List<Reward> rewards;
        }

        [SerializeField]
        public List<TutorialData> tutorials;

        [SerializeField]
        public List<RewardList> rewardLists;
    }
}
