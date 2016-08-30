using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SnoopySoundHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip levelStartSound;

    [SerializeField]
    private AudioClip winSound;

    [SerializeField]
    private AudioClip loseSound;

    [SerializeField]
    private AudioClip[] launchSounds;

    private AudioSource audioSource;
    private System.Random randomGenerator;

    public void Start()
    {
        randomGenerator = new System.Random();

        audioSource = GetComponent<AudioSource>();

        var eventService = GlobalState.EventService;
        eventService.AddEventHandler<LevelCompleteEvent>(OnLevelComplete);
        eventService.AddEventHandler<BubbleFiredEvent>(OnBubbleFired);
        eventService.AddEventHandler<IntroScrollCompleteEvent>(OnLevelStart);
    }

    private void OnLevelComplete(LevelCompleteEvent gameEvent)
    {
        PlaySound(gameEvent.Won ? winSound : loseSound);
    }

    private void OnBubbleFired(BubbleFiredEvent firedEvent)
    {
        int index = randomGenerator.Next(launchSounds.Length);
        PlaySound(launchSounds[index]);
    }

    private void OnLevelStart(IntroScrollCompleteEvent scrollEvent)
    {
        PlaySound(levelStartSound);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
