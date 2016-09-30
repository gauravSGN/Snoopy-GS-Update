using Model;
using UnityEngine;
using Snoopy.Characters;
using System.Collections.Generic;

namespace Modifiers
{
    [BubbleModifierAttribute(BubbleModifierType.RescueTarget)]
    public class RescueTargetModifier : EditorBubbleModifier
    {
        private const string PREFAB_PATH = "Characters/Woodstock";
        private const string SLIDEOUT_PATH = "Slideouts/SaveBirdsObjectiveSlideout";

        private GameObject prefab;
        private GameObject slideout;
        private bool introComplete;

        private readonly List<WoodstockEventHandler> targets = new List<WoodstockEventHandler>();

        override public BubbleModifierType ModifierType { get { return BubbleModifierType.RescueTarget; } }
        override public string SpriteName { get { return "RescueTarget"; } }

        override protected void ModifyGameObject(GameObject target, BubbleData.ModifierData data)
        {
            CreateInstance(target);
        }

        private void CreateInstance(GameObject parent)
        {
            prefab = prefab ?? GlobalState.AssetService.LoadAsset<GameObject>(PREFAB_PATH);

            var instance = GameObject.Instantiate(prefab);
            var target = instance.GetComponent<WoodstockEventHandler>();

            instance.name = "Rescue Target";
            target.Model = parent.GetComponent<BubbleModelBehaviour>().Model;
            instance.transform.SetParent(parent.transform, false);

            targets.Add(target);

            if (targets.Count == 1)
            {
                GlobalState.AssetService.LoadAssetAsync<GameObject>(SLIDEOUT_PATH, OnSlideoutLoaded);

                GlobalState.EventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
                GlobalState.EventService.AddEventHandler<LevelIntroCompleteEvent>(OnIntroScrollComplete);
            }
        }

        private void OnLevelLoaded()
        {
            GlobalState.EventService.RemoveEventHandler<LevelLoadedEvent>(OnLevelLoaded);

            var config = GlobalState.Instance.Config.woodstock;
            var targetPosition = GameObject.Find("Bird Landing Target").transform.localPosition;
            var landingSpots = new List<Vector3>();
            var count = targets.Count;
            var majorRadius = config.majorLandingRadius;
            var minorRadius = config.minorLandingRadius;
            var theta = Mathf.PI / Mathf.Max(1, (count - 1));
            var spread = theta * config.landingSpread;

            for (var index = 0; index < count; index++)
            {
                var angle = index * theta + Random.Range(-spread, spread);

                landingSpots.Add(new Vector3(
                    targetPosition.x + majorRadius * Mathf.Cos(angle),
                    targetPosition.y + minorRadius * Mathf.Sin(angle),
                    targetPosition.z + Mathf.Sin(angle)
                ));
            }

            for (var index = count - 1; index >= 0; index--)
            {
                count--;
                var next = Random.Range(0, count);

                targets[index].LandingSpot = landingSpots[next];
                landingSpots.RemoveAt(next);
            }
        }

        private void OnIntroScrollComplete()
        {
            GlobalState.EventService.RemoveEventHandler<LevelIntroCompleteEvent>(OnIntroScrollComplete);

            introComplete = true;

            ShowSlideout();
        }

        private void OnSlideoutLoaded(GameObject prefab)
        {
            slideout = prefab;
            ShowSlideout();
        }

        private void ShowSlideout()
        {
            if (introComplete && (slideout != null))
            {
                GlobalState.EventService.Dispatch(new Slideout.ShowSlideoutEvent(slideout));
            }
        }
    }
}
