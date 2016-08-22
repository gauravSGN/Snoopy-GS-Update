using Event;
using System;
using UI.Popup;
using System.Collections.Generic;

namespace Sequence
{
    public class WinLevel : BaseSequence<LevelState>
    {
        private LevelState levelState;

        override public void Begin(LevelState parameters)
        {
            this.levelState = parameters;

            GlobalState.EventService.Dispatch(new LevelCompleteEvent(true));
            UpdateUser();

            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

            GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
            GlobalState.EventService.Dispatch(new BubbleSettledEvent());
        }

        private void OnCullAllBubblesComplete(ReactionsFinishedEvent gameEvent)
        {
            GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);
            GlobalState.PopupService.EnqueueWithDelay(1.0f, new GenericPopupConfig
            {
                title = "Level Won",
                mainText = ("Score: " + levelState.score.ToString() + "\n" +
                            "Stars: " + GlobalState.User.levels[levelState.levelNumber].stars.ToString()),
                closeActions = new List<Action> { TransitionToReturnScene },
                affirmativeActions = new List<Action> { TransitionToReturnScene }
            });
        }

        private void UpdateUser()
        {
            var user = GlobalState.User;
            var highScore = Math.Max(levelState.score, user.levels[levelState.levelNumber].score);

            // Only set data if we have to so we can avoid dispatching extra calls to GS
            if (user.levels[levelState.levelNumber].score < highScore)
            {
                user.levels[levelState.levelNumber].score = highScore;
            }

            for (int starIndex = levelState.starValues.Length - 1; starIndex >= 0; starIndex--)
            {
                if (highScore >= levelState.starValues[starIndex])
                {
                    var newStars = (starIndex + 1);

                    if (user.levels[levelState.levelNumber].stars < newStars)
                    {
                        user.levels[levelState.levelNumber].stars = newStars;
                    }

                    break;
                }
            }

            if (levelState.levelNumber == user.maxLevel)
            {
                user.maxLevel++;
                GlobalState.SceneService.PostTransitionCallbacks.Add(DispatchMovePlayerAvatar);
            }
        }

        private void DispatchMovePlayerAvatar()
        {
            GlobalState.EventService.Dispatch(new MovePlayerAvatarEvent());
        }
    }
}