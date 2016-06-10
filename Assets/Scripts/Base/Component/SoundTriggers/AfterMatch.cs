using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AfterMatch : MonoBehaviour
{
    public AudioClip[] sounds;
    public ThreshholdCondition condition;
    public int bubbleMatchThreshold;
    public float chanceToPlay;

    public enum ThreshholdCondition
    {
        EqualTo,
        GreaterThanOrEqualTo
    }

    private AudioSource audio;
    private System.Random rnd = new System.Random();
    private int counter = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        EventDispatcher.Instance.AddEventHandler<BubbleReactionEvent>(OnBubbleReactionEvent);
        EventDispatcher.Instance.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    void OnBubbleReactionEvent(BubbleReactionEvent gameEvent)
    {
        counter++;
    }

    void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        if ((((condition == ThreshholdCondition.EqualTo) && (counter == bubbleMatchThreshold)) ||
             ((condition == ThreshholdCondition.GreaterThanOrEqualTo) && (counter >= bubbleMatchThreshold))) &&
            (UnityEngine.Random.value <= chanceToPlay))
        {
            audio.PlayOneShot(sounds[rnd.Next(sounds.Length)], 0.7f);
        }

        counter = 0;
    }
}