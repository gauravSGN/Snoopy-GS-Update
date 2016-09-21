using UnityEngine;

namespace Sound
{
    sealed public class PlayMusicEvent : GameEvent
    {
        public AudioClip clip;
        public bool loop;

        public PlayMusicEvent(AudioClip clip, bool loop)
        {
            this.clip = clip;
            this.loop = loop;
        }

        public PlayMusicEvent(MusicType type, bool loop)
        {
            clip = GlobalState.SoundService.GetMusicByType(type);
            this.loop = loop;
        }
    }
}
