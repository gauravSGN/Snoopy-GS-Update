using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FTUE
{
    sealed public class TutorialProgressController : MonoBehaviour
    {
        [Serializable]
        public struct EventTriggerMapping
        {
            public string eventName;
            public TutorialTrigger trigger;
        }

        [SerializeField]
        private List<EventTriggerMapping> mappings;

        private readonly List<PowerUps.PowerUpType> activePowerUps = new List<PowerUps.PowerUpType>();

        public void Start()
        {
            GlobalState.EventService.Persistent.AddEventHandler<PowerUpFilledEvent>(OnPowerUpFilled);
            GlobalState.EventService.Persistent.AddEventHandler<PowerUpUsedEvent>(OnPowerUpUsed);

            SceneManager.sceneLoaded += OnSceneLoaded;

            foreach (var mapping in mappings)
            {
                RegisterMapping(mapping.eventName, mapping.trigger);
            }
        }

        private void RegisterMapping(string eventName, TutorialTrigger trigger)
        {
            var eventType = Type.GetType(eventName);

            GlobalState.EventService.Persistent.AddEventHandler(eventType, () => { DispatchTrigger(trigger); });
        }

        private void DispatchTrigger(TutorialTrigger trigger)
        {
            GlobalState.EventService.Dispatch(new TutorialProgressEvent(trigger));
        }

        private void OnPowerUpFilled(PowerUpFilledEvent gameEvent)
        {
            if (!activePowerUps.Contains(gameEvent.type))
            {
                activePowerUps.Add(gameEvent.type);

                var identifiers = activePowerUps.Select(t => t.ToString().Substring(0, 1)).OrderBy(c => c).ToArray();
                var identifier = string.Join(string.Empty, identifiers);

                GlobalState.EventService.Dispatch(new TutorialProgressEvent(TutorialTrigger.PowerUpFill, identifier));
            }
        }

        private void OnPowerUpUsed(PowerUpUsedEvent gameEvent)
        {
            activePowerUps.Remove(gameEvent.type);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if ((scene.name == "Map") || (scene.name == StringConstants.Scenes.MAP))
            {
                GlobalState.EventService.Dispatch(new TutorialProgressEvent(TutorialTrigger.SagaMap));
            }
        }
    }
}
