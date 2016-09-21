using UnityEngine;
using System.Collections.Generic;

sealed public class BubbleModifierController : MonoBehaviour
{
    public delegate void ModifyShot(GameObject bubble);

    public int Count { get { return shotModifierTypes.Count; } }

    private readonly List<ModifyShot> shotModifiers = new List<ModifyShot>();
    private readonly HashSet<ShotModifierType> shotModifierTypes = new HashSet<ShotModifierType>();
    private GameObject shooterBubble;
    private GameObject currentAnimation;

    public void Start()
    {
        GlobalState.EventService.AddEventHandler<SetShooterBubbleEvent>(OnSetShooterBubble);
        GlobalState.EventService.AddEventHandler<BubbleFiringEvent>(OnBubbleFiring);
    }

    public void Add(ModifyShot modifier, ShotModifierType type)
    {
        shotModifiers.Add(modifier);
        shotModifierTypes.Add(type);

        GlobalState.EventService.Dispatch(new AddShotModifierEvent(type));
    }

    public void SetAnimation(GameObject bubbleAnimation)
    {
        currentAnimation = bubbleAnimation;
        UpdateAnimationTransform();
    }

    public bool Contains(ShotModifierType type)
    {
        return shotModifierTypes.Contains(type);
    }

    private void UpdateAnimationTransform()
    {
        currentAnimation.transform.SetParent(shooterBubble.transform);
        currentAnimation.transform.localPosition = new Vector3(0, 0, -0.01f);
    }

    private void OnSetShooterBubble(SetShooterBubbleEvent gameEvent)
    {
        shooterBubble = gameEvent.bubble;

        if (currentAnimation != null)
        {
            UpdateAnimationTransform();
        }
    }

    private void OnBubbleFiring()
    {
        foreach (var modifier in shotModifiers)
        {
            modifier(shooterBubble);
        }

        shotModifiers.Clear();
        shotModifierTypes.Clear();

        currentAnimation = null;
    }
}
