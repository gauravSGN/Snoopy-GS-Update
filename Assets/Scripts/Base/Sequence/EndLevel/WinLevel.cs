using Event;
using System;
using Service;
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

        private long oldStars;
        private LevelState levelState;
        private GameObject currentItem;
        private GameConfig.WinSequenceConfig config;
        private EventService eventService;

        override public void Begin(LevelState parameters)
        {
            this.levelState = parameters;
            config = GlobalState.Instance.Config.winSequence;

            eventService = GlobalState.EventService;

            eventService.Dispatch(new InputToggleEvent(false));
            eventService.Dispatch(new Sound.PlayMusicEvent(Sound.MusicType.WinLevel, true));

            StartCoroutine(RunActionAfterDelay(config.delayBeforeCullAll, () =>
            {
                eventService.Dispatch(new LevelCompleteEvent(true));

                eventService.AddEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

                eventService.Dispatch(new CullAllBubblesEvent());
                eventService.Dispatch(new BubbleSettledEvent());
            }));
        }

        private void OnCullAllBubblesComplete()
        {
            eventService.RemoveEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);
            eventService.AddEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);
            eventService.Dispatch(new PrepareForBubblePartyEvent());

            // Put back the ball the character was holding and prepare to party
            levelState.remainingBubbles++;
            levelState.NotifyListeners();
            levelState.preparedForBubbleParty = true;

            StartCoroutine(RunActionAfterDelay(config.delayBeforeWinTextAnimation, () =>
            {
                currentItem = Instantiate(winTextAnimationPrefab);
                currentItem.transform.SetParent(canvas.transform, false);

                eventService.AddEventHandler<StartBubblePartyEvent>(OnStartBubbleParty);
            }));
        }

        private void OnWinTextAnimationComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (gameEvent.item == currentItem)
            {
                eventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnWinTextAnimationComplete);

                Destroy(currentItem);
                currentItem = null;

                StartCoroutine(RunActionAfterDelay(config.delayBeforeCelebration, () =>
                {
                    eventService.Dispatch(new StartWinAnimationsEvent());
                }));
            }
        }

        private void OnStartBubbleParty()
        {
            eventService.RemoveEventHandler<StartBubblePartyEvent>(OnStartBubbleParty);
            GlobalState.Instance.RunCoroutine(BubbleParty());
        }

        private IEnumerator BubbleParty()
        {
            var delayBetweenBubbles = GlobalState.Instance.Config.bubbleParty.delayBetweenBubbles;

            yield return new WaitForSeconds(config.delayBeforeBubbleParty);

            while (levelState.remainingBubbles > 0)
            {
                yield return new WaitForSeconds(delayBetweenBubbles);
                eventService.Dispatch(new FirePartyBubbleEvent());
            }

            StartCoroutine(RunActionAfterDelay(config.delayBeforeSlideOut, () =>
            {
                eventService.AddEventHandler<SlideoutCompleteEvent>(OnSlideoutComplete);
                eventService.AddEventHandler<SlideoutStartEvent>(OnSlideoutStart);
                eventService.Dispatch(new ShowSlideoutEvent(winSliderPrefab));
            }));
        }

        private void OnSlideoutStart(SlideoutStartEvent gameEvent)
        {
            currentItem = gameEvent.instance;
            eventService.RemoveEventHandler<SlideoutStartEvent>(OnSlideoutStart);
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
                    level = levelState.levelNumber,
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
                        oldStars = user.levels[levelState.levelNumber].stars;
                        user.levels[levelState.levelNumber].stars = newStars;
                        GlobalState.SceneService.OnFinishedLoading += DispatchAnimateStarsOnMapNode;
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
            eventService.Dispatch(new MovePlayerAvatarEvent());
        }

        private void DispatchAnimateStarsOnMapNode()
        {
            GlobalState.SceneService.OnFinishedLoading -= DispatchAnimateStarsOnMapNode;
            eventService.Dispatch(new AnimateStarsOnMapNodeEvent { oldStars = oldStars });
        }
    }
}
