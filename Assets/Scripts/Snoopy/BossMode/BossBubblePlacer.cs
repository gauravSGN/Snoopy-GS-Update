using Model;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Snoopy.BossMode
{
    sealed public class BossBubblePlacer : BubblePlacer
    {
        [SerializeField]
        private Transform bubbleContainer;

        private readonly List<Transform> bubbles = new List<Transform>();

        override public void Start()
        {
            base.Start();

            GlobalState.EventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
        }

        override public GameObject PlaceBubble(BubbleData data)
        {
            var instance = CreateBubble(data);
            var container = new GameObject();

            container.transform.SetParent(bubbleContainer, false);
            container.SetActive(false);

            instance.transform.SetParent(container.transform, false);

            container.transform.localPosition = GetBubbleLocation(data.X, data.Y);
            instance.transform.position = transform.position;

            bubbles.Add(instance.transform);

            return instance;
        }

        private void OnLevelLoaded()
        {
            GlobalState.EventService.RemoveEventHandler<LevelLoadedEvent>(OnLevelLoaded);
            GlobalState.EventService.Dispatch(new InputToggleEvent(false));

            StartCoroutine(MoveBubbles());
        }

        private IEnumerator MoveBubbles()
        {
            var releaseRate = 3.0f / bubbles.Count;
            var timer = 0.0f;
            var active = 0;
            var complete = 0;

            bubbles.Sort((a, b) => b.localPosition.sqrMagnitude.CompareTo(a.localPosition.sqrMagnitude));

            yield return null;

            while (bubbles.Count > 0)
            {
                var deltaTime = Time.deltaTime;
                timer += deltaTime;

                while (((active - complete) < bubbles.Count) && (timer >= (active * releaseRate)))
                {
                    bubbles[active - complete].parent.gameObject.SetActive(true);
                    active++;
                }

                for (var index = 0; index < (active - complete); index++)
                {
                    var result = Vector3.MoveTowards(bubbles[index].localPosition, Vector3.zero, deltaTime);
                    bubbles[index].localPosition = result;

                    if (result.sqrMagnitude < Mathf.Epsilon)
                    {
                        var container = bubbles[index].parent;
                        bubbles[index].SetParent(bubbleContainer, true);
                        Destroy(container.gameObject);

                        bubbles.RemoveAt(index);
                        complete++;
                        index--;
                    }
                }

                yield return null;
            }

            GlobalState.EventService.Dispatch(new LevelIntroCompleteEvent());
            GlobalState.EventService.Dispatch(new InputToggleEvent(true));
        }
    }
}
