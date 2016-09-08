using Event;
using System;
using Slideout;
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
        private GameObject winSliderPrefab;

        [SerializeField]
        private GameObject winTextAnimationPrefab;

        private LevelState levelState;
        private GameObject currentItem;
        private GameConfig.WinSequenceConfig config;

        override public void Begin(LevelState parameters)
        {
            this.levelState = parameters;
            config = GlobalState.Instance.Config.winSequence;

            GlobalState.EventService.Dispatch(new Sound.PlayMusicEvent(Sound.MusicType.WinLevel, true));

            StartCoroutine(RunActionAfterDelay(config.delayBeforeCullAll, () =>
            {
                GlobalState.EventService.Dispatch(new LevelCompleteEvent(true));

                GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

                GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
                GlobalState.EventService.Dispatch(new BubbleSettledEvent());
            }));
        }

        private void OnCullAllBubblesComplete()
        {
            GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);
            GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);
            GlobalState.EventService.Dispatch(new PrepareForBubblePartyEvent());

            // Put back the ball the character was holding and prepare to party
            levelState.remainingBubbles++;
            levelState.NotifyListeners();
            levelState.preparedForBubbleParty = true;

            StartCoroutine(RunActionAfterDelay(config.delayBeforeWinTextAnimation, () =>
            {
                currentItem = Instantiate(winTextAnimationPrefab);
                currentItem.transform.SetParent(canvas.transform, false);

                GlobalState.EventService.AddEventHandler<StartBubblePartyEvent>(OnStartBubbleParty);
            }));
        }

        private void OnWinTextAnimationComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (gameEvent.item == currentItem)
            {
                GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);

                Destroy(currentItem);
                currentItem = null;

                StartCoroutine(RunActionAfterDelay(config.delayBeforeCelebration, () =>
                {
                    GlobalState.EventService.Dispatch(new StartWinAnimationsEvent());
                }));
            }
        }

        private void OnStartBubbleParty(StartBubblePartyEvent gameEvent)
        {
            GlobalState.EventService.RemoveEventHandler<StartBubblePartyEvent>(OnStartBubbleParty);
            GlobalState.Instance.RunCoroutine(BubbleParty());
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

            StartCoroutine(RunActionAfterDelay(config.delayBeforeSlideOut, () =>
            {
                GlobalState.EventService.AddEventHandler<SlideoutCompleteEvent>(OnSlideoutComplete);
                GlobalState.EventService.AddEventHandler<SlideoutStartEvent>(OnSlideoutStart);
                GlobalState.EventService.Dispatch(new ShowSlideoutEvent(winSliderPrefab));
            }));
        }

        private void OnSlideoutStart(SlideoutStartEvent gameEvent)
        {
            currentItem = gameEvent.instance;
            GlobalState.EventService.RemoveEventHandler<SlideoutStartEvent>(OnSlideoutStart);
        }

        private void OnSlideoutComplete(SlideoutCompleteEvent gameEvent)
        {
            if (gameEvent.instance == currentItem)
            {
                UpdateUser();

                var userLevelData = GlobalState.User.levels[levelState.levelNumber];

                GlobalState.PopupService.EnqueueWithDelay(config.delayBeforePopup, new WinLevelPopupConfig
                {
                    score = levelState.score,
                    stars = userLevelData.stars,
                    topScore = userLevelData.score,
                    closeActions = new List<Action> { TransitionToReturnScene },
                    affirmativeActions = new List<Action> { TransitionToReturnScene }
                });
            }
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
                GlobalState.SceneService.OnFinishedLoading += DispatchMovePlayerAvatar;
            }
        }

        private void DispatchMovePlayerAvatar()
        {
            GlobalState.SceneService.OnFinishedLoading -= DispatchMovePlayerAvatar;
            GlobalState.EventService.Dispatch(new MovePlayerAvatarEvent());
        }
    }
}
