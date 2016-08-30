using UnityEngine;

namespace Sound
{
    sealed public class PlaySoundEvent : GameEvent
    {
        public AudioClip clip;

        public PlaySoundEvent(AudioClip clip)
        {
            this.clip = clip;
        }

        public PlaySoundEvent(SoundType type)
        {
            clip = GlobalState.SoundService.GetSoundByType(type);
        }
    }
}
