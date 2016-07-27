using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    sealed public class LevelConfiguration
    {
        public System.Random RandomNumberGenerator { get; private set; }
        public Dictionary<BubbleType, int> Counts { get; private set; }
        public ChainedRandomizer<BubbleType>[] Randoms { get; private set; }

        public LevelConfiguration(LevelData data)
        {
            RandomNumberGenerator = new System.Random();
            Counts = new Dictionary<BubbleType, int>();
            Randoms = CreateRandomizers(RandomNumberGenerator, data.Randoms);
        }

        public static ChainedRandomizer<BubbleType>[] CreateRandomizers(System.Random rng,
                                                                        RandomBubbleDefinition[] randoms)
        {
            var count = randoms.Length;
            var result = new ChainedRandomizer<BubbleType>[count];

            for (var index = 0; index < count; index++)
            {
                var group = randoms[index];

                result[index] = new ChainedRandomizer<BubbleType>(
                    rng,
                    group.rollType,
                    BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES,
                    group.weights.Select(w => (float)w)
                );
            }

            for (var index = 0; index < count; index++)
            {
                foreach (var exclusion in randoms[index].exclusions)
                {
                    result[index].AddExclusion(result[exclusion]);
                }
            }

            return result;
        }
    }
}
