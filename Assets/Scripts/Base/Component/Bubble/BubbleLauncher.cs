using UnityEngine;
using System.Collections.Generic;
using Service;

public class BubbleLauncher : MonoBehaviour
{
    public delegate void ModifyShot(GameObject bubble);

    [SerializeField]
    private GameObject[] locations;

    [SerializeField]
    private float launchSpeed;

    [SerializeField]
    private Level level;

    [SerializeField]
    private AimLine aimLine;

    private GameObject[] nextBubbles;
    private List<ModifyShot> shotModifiers;
    private GameObject currentAnimation;

    public void CycleQueue()
    {
        CycleLocalQueue();
        level.levelState.bubbleQueue.Rotate(nextBubbles.Length);
    }

    public void AddShotModifier(ModifyShot modifier)
    {
        shotModifiers.Add(modifier);
    }

    public void SetModifierAnimation(GameObject bubbleAnimation)
    {
        currentAnimation = bubbleAnimation;
        currentAnimation.transform.parent = nextBubbles[0].transform;
    }

    protected void Start()
    {
        nextBubbles = new GameObject[locations.Length];
        shotModifiers = ResetShotModifiers();

        CreateBubbles();
        SetAimLineColor();

        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubbleEvent);
    }

    protected void OnMouseUp()
    {
        if (nextBubbles[0] != null && aimLine.Aiming)
        {
            FireBubbleAt(aimLine.Target);
        }
    }

    private void CreateBubbles()
    {
        for (var index = 0; index < nextBubbles.Length; index++)
        {
            if (nextBubbles[index] == null)
            {
                nextBubbles[index] = level.bubbleFactory.CreateByType(level.levelState.bubbleQueue.Peek(index));
                MoveBubbleToLocation(index);
            }
        }
    }

    private void FireBubbleAt(Vector2 point)
    {
        foreach (var modifier in shotModifiers)
        {
            modifier(nextBubbles[0]);
        }

        var direction = (point - (Vector2)locations[0].transform.position).normalized * launchSpeed;
        var rigidBody = nextBubbles[0].GetComponent<Rigidbody2D>();
        nextBubbles[0].transform.parent = null;

        rigidBody.isKinematic = false;
        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new BubbleFiredEvent(nextBubbles[0]));

        nextBubbles[0] = null;
        currentAnimation = null;
        shotModifiers = ResetShotModifiers();
    }

    private void ReadyNextBubble()
    {
        level.levelState.bubbleQueue.GetNext();

        CycleLocalQueue();
        CreateBubbles();
    }

    private void MoveBubbleToLocation(int index)
    {
        nextBubbles[index].transform.parent = locations[index].transform;
        nextBubbles[index].transform.localPosition = Vector3.zero;
    }

    private void CycleLocalQueue()
    {
        var lastIndex = nextBubbles.Length - 1;
        var first = nextBubbles[0];

        for (var index = 0; index < lastIndex; index++)
        {
            nextBubbles[index] = nextBubbles[index + 1];
            if (nextBubbles[index] != null)
            {
                MoveBubbleToLocation(index);
            }
        }

        nextBubbles[lastIndex] = first;
        if (first != null)
        {
            MoveBubbleToLocation(lastIndex);
        }

        if (currentAnimation != null)
        {
            currentAnimation.transform.parent = null;
            currentAnimation.transform.parent = nextBubbles[0].transform;
        }

        SetAimLineColor();
    }

    private void SetAimLineColor()
    {
        aimLine.Color = nextBubbles[0].GetComponent<BubbleAttachments>().Model.definition.BaseColor;
    }

    private void OnReadyForNextBubbleEvent(ReadyForNextBubbleEvent gameEvent)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        ReadyNextBubble();
    }

    private List<ModifyShot> ResetShotModifiers()
    {
        return new List<ModifyShot>{AddBubbleSnap};
    }

    private void AddBubbleSnap(GameObject bubble)
    {
        bubble.AddComponent<BubbleSnap>();
    }
}
