using Model;
using UnityEngine;
using System.Collections.Generic;

public class BubblePartyLauncher : MonoBehaviour
{
    [SerializeField]
    private Level level;

    [SerializeField]
    private GameObject launchLocation;

    private readonly List<BubbleType> possibleBubbleTypes = new List<BubbleType>();
    private readonly RandomBag<BubbleType> bubbleTypePicker = new RandomBag<BubbleType>();

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
        rigidBody.AddForce(new Vector3(Random.Range(bubblePartyConfig.minXForce, bubblePartyConfig.maxXForce),
                                       Random.Range(bubblePartyConfig.minYForce, bubblePartyConfig.maxYForce)));

        nextBubble.GetComponent<BubbleModelBehaviour>().Model.MakeBubbleFall();

        // Put it on the default layer since we don't want it to get culled by the floor as soon as it comes out.
        nextBubble.layer = (int)Layers.Default;

        Sound.PlaySoundEvent.Dispatch(Sound.SoundType.LaunchBubble);
        level.levelState.DecrementRemainingBubbles();
    }

    private GameObject CreateNextBubble()
    {
        if (bubbleTypePicker.Empty)
        {
            bubbleTypePicker.AddRange(possibleBubbleTypes);
        }

        var bubble = level.bubbleFactory.CreateByType(bubbleTypePicker.Next());

        bubble.transform.parent = launchLocation.transform;
        bubble.transform.position = launchLocation.transform.position;
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