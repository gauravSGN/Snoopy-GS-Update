using System;
using Service;
using UnityEngine;
using System.Collections.Generic;

namespace Sound
{
    sealed public class SoundManager : MonoBehaviour, SoundService, UpdateReceiver
    {
        [Serializable]
        private class BaseSoundEntry<T>
        {
            [SerializeField]
            public T type;

            [SerializeField]
            public string resource;
        }

        [Serializable]
        private class SoundEntry : BaseSoundEntry<SoundType> {}

        [Serializable]
        private class MusicEntry : BaseSoundEntry<MusicType> {}

        [SerializeField]
        private int initialChannelCount;

        [SerializeField]
        private List<SoundEntry> defaultSounds;

        [SerializeField]
        private List<MusicEntry> defaultMusic;

        private Dictionary<SoundType, string> soundLookup;
        private Dictionary<MusicType, string> musicLookup;
        private readonly Dictionary<string, AudioClip> soundCache = new Dictionary<string, AudioClip>();

        private readonly List<AudioSource> freeChannels = new List<AudioSource>();
        private readonly List<AudioSource> activeChannels = new List<AudioSource>();

        private AssetService assetService;
        private AudioSource musicChannel;

        public bool SoundMuted { get; private set; }
        public bool MusicMuted { get; private set; }
        public bool MusicPlaying { get { return musicChannel != null; } }
        public int SoundsPlaying { get; private set; }

        public void Start()
        {
            assetService = GlobalState.AssetService;

            while (freeChannels.Count < initialChannelCount)
            {
                freeChannels.Add(CreateChannel());
            }

            GlobalState.Instance.Services.SetInstance<SoundService>(this);

            GlobalState.EventService.AddEventHandler<PlaySoundEvent>(OnPlaySound, Event.HandlerDictType.Persistent);
            GlobalState.EventService.AddEventHandler<PlayMusicEvent>(OnPlayMusic, Event.HandlerDictType.Persistent);
        }

        public void OnUpdate()
        {
            var index = 0;

            while (index < activeChannels.Count)
            {
                if (!activeChannels[index].isPlaying)
                {
                    var channel = activeChannels[index];

                    channel.Stop();
                    channel.clip = null;

                    freeChannels.Add(channel);
                    activeChannels.RemoveAt(index);

                    if (channel == musicChannel)
                    {
                        musicChannel = null;
                    }
                }
                else
                {
                    index++;
                }
            }

            if (activeChannels.Count == 0)
            {
                GlobalState.UpdateService.Updates.Remove(this);
            }
        }

        public void PlaySound(AudioClip clip)
        {
            if (!SoundMuted)
            {
                PlayClip(clip);
            }
        }

        public void PlaySound(SoundType type)
        {
            PlaySound(GetSoundByType(type));
        }

        public void PlayMusic(AudioClip clip, bool loop)
        {
            if (!MusicMuted)
            {
                StopMusic();

                musicChannel = PlayClip(clip);
                musicChannel.loop = loop;
            }
        }

        public void PlayMusic(MusicType type, bool loop)
        {
            PlayMusic(GetMusicByType(type), loop);
        }

        public void StopMusic()
        {
            if (musicChannel != null)
            {
                musicChannel.Stop();
                musicChannel = null;
            }
        }

        public void PreloadSound(SoundType type)
        {
            GetSoundByType(type);
        }

        public AudioClip GetSoundByType(SoundType type)
        {
            soundLookup = soundLookup ?? BuildLookup<SoundEntry, SoundType>(defaultSounds);

            return LoadClipByType(soundLookup, type);
        }

        public AudioClip GetMusicByType(MusicType type)
        {
            musicLookup = musicLookup ?? BuildLookup<MusicEntry, MusicType>(defaultMusic);

            return LoadClipByType(musicLookup, type);
        }

        private void OnLevelWasLoaded(int level)
        {
            soundCache.Clear();
        }

        private Dictionary<U, string> BuildLookup<T, U>(IEnumerable<T> entries) where T : BaseSoundEntry<U>
        {
            var result = new Dictionary<U, string>();

            foreach (var entry in entries)
            {
                result.Add(entry.type, entry.resource);
            }

            return result;
        }

        private AudioClip LoadClipByType<T>(Dictionary<T, string> lookup, T type)
        {
            string name = null;

            if (lookup.TryGetValue(type, out name))
            {
                if (!soundCache.ContainsKey(name))
                {
                    soundCache.Add(name, assetService.LoadAsset<AudioClip>(name));
                }
            }

            return soundCache.ContainsKey(name) ? soundCache[name] : null;
        }

        private AudioSource CreateChannel()
        {
            var channel = gameObject.AddComponent<AudioSource>();

            channel.playOnAwake = false;

            return channel;
        }

        private AudioSource PlayClip(AudioClip clip)
        {
            var channel = GetFreeChannel();

            channel.clip = clip;
            channel.loop = false;
            channel.Play();

            activeChannels.Add(channel);

            if (activeChannels.Count == 1)
            {
                GlobalState.UpdateService.Updates.Add(this);
            }

            return channel;
        }

        private AudioSource GetFreeChannel()
        {
            if (freeChannels.Count == 0)
            {
                freeChannels.Add(CreateChannel());
            }

            var channel = freeChannels[0];
            freeChannels.RemoveAt(0);

            return channel;
        }

        private void OnPlaySound(PlaySoundEvent gameEvent)
        {
            PlaySound(gameEvent.clip);
        }

        private void OnPlayMusic(PlayMusicEvent gameEvent)
        {
            PlayMusic(gameEvent.clip, gameEvent.loop);
        }
    }
}
