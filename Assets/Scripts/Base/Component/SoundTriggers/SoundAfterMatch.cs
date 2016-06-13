using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundAfterMatch : MonoBehaviour
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

    private AudioSource audioSource;
    private System.Random rnd = new System.Random();
    private int counter = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventDispatcher.Instance.AddEventHandler<BubbleReactionEvent>(OnBubbleReactionEvent);
        EventDispatcher.Instance.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    void OnBubbleReactionEvent(BubbleReactionEvent gameEvent)
    {
        counter++;
    }

    void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        if (ConditionMet() && (UnityEngine.Random.value <= chanceToPlay))
        {
            audioSource.PlayOneShot(sounds[rnd.Next(sounds.Length)], 0.7f);
        }

        counter = 0;
    }

    bool ConditionMet()
    {
        bool equalToConditionMet = ((condition == ThreshholdCondition.EqualTo) && (counter == bubbleMatchThreshold));
        bool greaterThanOrEqualToConditionMet = ((condition == ThreshholdCondition.GreaterThanOrEqualTo) && (counter >= bubbleMatchThreshold));

        return equalToConditionMet || greaterThanOrEqualToConditionMet;
    }
}