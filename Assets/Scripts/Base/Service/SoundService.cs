using Sound;
using UnityEngine;

namespace Service
{
    public interface SoundService : SharedService
    {
        bool SoundMuted { get; }
        bool MusicMuted { get; }

        bool MusicPlaying { get; }
        int SoundsPlaying { get; }

        void PlaySound(AudioClip clip);
        void PlaySound(SoundType type);

        void PlayMusic(AudioClip clip, bool loop);
        void PlayMusic(MusicType type, bool loop);
        void StopMusic();

        void PreloadSound(SoundType type);

        AudioClip GetSoundByType(SoundType type);
        AudioClip GetMusicByType(MusicType type);
    }
}
