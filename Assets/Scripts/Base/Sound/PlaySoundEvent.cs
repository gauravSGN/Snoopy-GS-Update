﻿using UnityEngine;

namespace Sound
{
    sealed public class PlaySoundEvent : GameEvent
    {
        public AudioClip clip;

        public static void Dispatch(AudioClip clip)
        {
            var gameEvent = GlobalState.EventService.GetPooledEvent<PlaySoundEvent>();

            gameEvent.clip = clip;

            GlobalState.EventService.DispatchPooled(gameEvent);
        }

        public static void Dispatch(SoundType type)
        {
            var gameEvent = GlobalState.EventService.GetPooledEvent<PlaySoundEvent>();

            gameEvent.clip = GlobalState.SoundService.GetSoundByType(type);

            GlobalState.EventService.DispatchPooled(gameEvent);
        }
    }
}
