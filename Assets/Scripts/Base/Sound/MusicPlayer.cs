using UnityEngine;
using System.Collections;

namespace Sound
{
    sealed public class MusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private MusicType type;

        [SerializeField]
        private bool playOnAwake;

        [SerializeField]
        private bool loop;

        public void Start()
        {
            if (playOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            StartCoroutine(PlayNextFrame());
        }

        private IEnumerator PlayNextFrame()
        {
            yield return null;

            GlobalState.EventService.Dispatch(new PlayMusicEvent(type, loop));
        }
    }
}
