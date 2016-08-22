using Model;
using UnityEngine;
using System.Collections.Generic;

public class BubblePartyLauncher : MonoBehaviour
{
    [SerializeField]
    private Level level;

    [SerializeField]
    private GameObject launchLocation;

    [SerializeField]
    private AudioSource launchSound;

    private List<BubbleType> possibleBubbleTypes = new List<BubbleType>();
    private RandomBag<BubbleType> bubbleTypePicker = new RandomBag<BubbleType>();

    protected void Start()
    {
        GlobalState.EventService.AddEventHandler<FirePartyBubbleEvent>(OnFirePartyBubble);
        GlobalState.EventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
    }

    private void OnPrepareForBubbleParty(PrepareForBubblePartyEvent gameEvent)
    {
        FindPossibleBubbleTypes();
    }

    private void OnFirePartyBubble(FirePartyBubbleEvent gameEvent)
    {
        var nextBubble = CreateNextBubble();
        var rigidBody = nextBubble.GetComponent<Rigidbody2D>();
        var bubblePartyConfig = GlobalState.Instance.Config.bubbleParty;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector3(UnityEngine.Random.Range(bubblePartyConfig.minXForce, bubblePartyConfig.maxXForce),
                                       UnityEngine.Random.Range(bubblePartyConfig.minYForce, bubblePartyConfig.maxYForce)));

        launchSound.Play();
        level.levelState.DecrementRemainingBubbles();
    }

    private GameObject CreateNextBubble()
    {
        var offset = new Vector3(0, -GlobalState.Instance.Config.bubbles.size / 2);

        if (bubbleTypePicker.Empty)
        {
            bubbleTypePicker.AddRange(possibleBubbleTypes);
        }

        var bubble = level.bubbleFactory.CreateByType(bubbleTypePicker.Next());

        bubble.transform.parent = launchLocation.transform;
        bubble.transform.position = launchLocation.transform.position + offset;
        bubble.layer = (int)Layers.Default;
        bubble.AddComponent<BubbleParty>();

        return bubble;
    }

    private void FindPossibleBubbleTypes()
    {
        foreach (var bubbleType in BaseBubbleQueue.LAUNCHER_BUBBLE_TYPES)
        {
            if (level.levelState.initialTypeTotals.ContainsKey(bubbleType))
            {
                possibleBubbleTypes.Add(bubbleType);
            }
        }
    }
}