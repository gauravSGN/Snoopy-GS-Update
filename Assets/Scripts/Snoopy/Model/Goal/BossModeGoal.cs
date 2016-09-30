using Goal;
using Paths;
using Model;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Snoopy.Model.Goal
{
    sealed public class BossModeGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.BossMode; } }

        override public void Initialize(LevelData levelData)
        {
            var trackBubbles = levelData.Bubbles.Where(b => b.Type == BubbleType.BossTrack).ToList();

            if (trackBubbles.Count > 0)
            {
                var path = new NodeTrackPath();

                foreach (var bubble in trackBubbles)
                {
                    path.AddNode(bubble.X, bubble.Y, bubble.WorldPosition);
                }

                var start = GetStartModifier(trackBubbles);

                if (start != null)
                {
                    var modifier = start.modifiers.First(m => m.type == BubbleModifierType.BossStartPosition);
                    path.Start(start.X, start.Y, int.Parse(modifier.data));
                }

                GlobalState.EventService.Dispatch(new Snoopy.BossMode.SetBossPathEvent(path));
            }
        }

        private BubbleData GetStartModifier(IEnumerable<BubbleData> bubbles)
        {
            foreach (var bubble in bubbles)
            {
                if ((bubble.modifiers != null) &&
                    bubble.modifiers.Any(m => m.type == BubbleModifierType.BossStartPosition))
                {
                    return bubble;
                }
            }

            return null;
        }
    }
}
