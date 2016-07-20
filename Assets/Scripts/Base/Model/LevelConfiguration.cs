using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    sealed public class LevelConfiguration
    {
        public System.Random RNG { get; private set; }
        public Dictionary<BubbleType, int> Counts { get; private set; }
        public ChainedRandomizer<BubbleType>[] Randoms { get; private set; }

        public LevelConfiguration(LevelData data)
        {
            RNG = new System.Random();
            Counts = new Dictionary<BubbleType, int>();

            CreateRandomizers(data);
        }

        private void CreateRandomizers(LevelData data)
        {
            var count = data.Randoms.Length;
            Randoms = new ChainedRandomizer<BubbleType>[count];

            for (var index = 0; index < count; index++)
            {
                var group = data.Randoms[index];

                Randoms[index] = new ChainedRandomizer<BubbleType>(
                    RNG,
                    group.rollType,
                    BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES,
                    group.weights.Select(w => (float)w)
                );
            }

            for (var index = 0; index < count; index++)
            {
                foreach (var exclusion in data.Randoms[index].exclusions)
                {
                    Randoms[index].AddExclusion(Randoms[exclusion]);
                }
            }
        }
    }
}
