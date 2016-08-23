using Event;
using System;
using UI.Popup;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class WinLevel : BaseSequence<LevelState>
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private GameObject winTextAnimationPrefab;

        private LevelState levelState;
        private GameObject winTextAnimation;
        private GameConfig.WinSequenceConfig config;

        override public void Begin(LevelState parameters)
        {
            this.levelState = parameters;
            config = GlobalState.Instance.Config.winSequence;

            StartCoroutine(RunActionAfterDelay(config.delayBeforeCullAll, () =>
            {
                GlobalState.EventService.Dispatch(new LevelCompleteEvent(true));
                UpdateUser();

                GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

                GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
                GlobalState.EventService.Dispatch(new BubbleSettledEvent());
            }));
        }

        private void OnCullAllBubblesComplete(ReactionsFinishedEvent gameEvent)
        {
            GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);
            GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);
            GlobalState.EventService.Dispatch(new PrepareForBubblePartyEvent());

            // Put back the ball the character was holding
            levelState.remainingBubbles++;
            levelState.NotifyListeners();

            StartCoroutine(RunActionAfterDelay(config.delayBeforeWinTextAnimation, () =>
            {
                winTextAnimation = Instantiate(winTextAnimationPrefab);
                winTextAnimation.transform.SetParent(canvas.transform, false);
            }));
        }

        private void OnWinTextAnimationComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (gameEvent.item == winTextAnimation)
            {
                GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);
                StartCoroutine(RunActionAfterDelay(config.delayBeforeCelebration, () =>
                {
                    GlobalState.EventService.Dispatch(new Sequence.StartWinAnimationsEvent());
                    GlobalState.Instance.RunCoroutine(BubbleParty());
                }));
            }
        }

        private IEnumerator BubbleParty()
        {
            var delayBetweenBubbles = GlobalState.Instance.Config.bubbleParty.delayBetweenBubbles;

            yield return new WaitForSeconds(config.delayBeforeBubbleParty);

            while (levelState.remainingBubbles > 0)
            {
                yield return new WaitForSeconds(delayBetweenBubbles);
                GlobalState.EventService.Dispatch(new FirePartyBubbleEvent());
            }

            GlobalState.PopupService.EnqueueWithDelay(config.delayBeforePopup, new GenericPopupConfig
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