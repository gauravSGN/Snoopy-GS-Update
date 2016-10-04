using Goal;
using Paths;
using Model;
using System.Linq;
using UnityEngine;
using Snoopy.BossMode;
using System.Collections.Generic;

namespace Snoopy.Model.Goal
{
    sealed public class BossModeGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.BossMode; } }

        private LevelData data;

        override public void Initialize(LevelData levelData)
        {
            var trackBubbles = levelData.Bubbles.Where(b => b.Type == BubbleType.BossTrack).ToList();

            if (trackBubbles.Count > 0)
            {
                data = levelData;

                TargetValue = levelData.Puzzles.Length;
                Score = GetScoreForGoalType(Type);

                GlobalState.EventService.AddEventHandler<DamageBossEvent>(OnDamageBoss);

                GlobalState.EventService.Dispatch(new SetBossHealthEvent(TargetValue));
                UpdateBossPath();
            }
        }

        private void UpdateBossPath()
        {
            var trackBubbles = data.Bubbles.Where(b => b.Type == BubbleType.BossTrack).ToList();
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

            GlobalState.EventService.Dispatch(new SetBossPathEvent(path));
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

        private void OnDamageBoss()
        {
            GlobalState.EventService.Dispatch(new GoalIncrementEvent(this, null, Score));

            CurrentValue++;
            NotifyListeners();

            if (CurrentValue == TargetValue)
            {
                CompleteGoal();
            }
            else
            {
                GlobalState.EventService.AddEventHandler<Event.ReactionsFinishedEvent>(OnReactionsFinished);
            }
        }

        private void OnReactionsFinished()
        {
            GlobalState.EventService.RemoveEventHandler<Event.ReactionsFinishedEvent>(OnReactionsFinished);
            GlobalState.EventService.Dispatch(new StartNextPuzzleEvent());

            UpdateBossPath();
        }
    }
}
