using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
sealed public class BubbleSoundPlayer : MonoBehaviour
{
    private AudioSource source;

    public void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if ((clip != null) && GlobalState.User.settings.sfxOn)
        {
            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }
}
