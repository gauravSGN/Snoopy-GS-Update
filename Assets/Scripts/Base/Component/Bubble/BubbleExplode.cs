using UnityEngine;

public class BubbleExplode : MonoBehaviour
{
    private const float BUBBLE_SPACING = 0.225f;

    [SerializeField]
    private int sizeMultiplier = 2;

    public void Start()
    {
        EventDispatcher.Instance.AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize)
    {
        sizeMultiplier = explosionSize;
    }

    public void OnSettling(GameEvent gameEvent)
    {
        if (Physics2D.CircleCastAll(transform.position, BUBBLE_SPACING * sizeMultiplier, Vector2.up, 0.0f).Length > 0)
        {
            BubbleReactionEvent.Dispatch(ReactionPriority.Explode, this.Explode);
        }
    }

    public void Explode()
    {
        EventDispatcher.Instance.RemoveEventHandler<BubbleSettlingEvent>(OnSettling);

        foreach (var hit in Physics2D.CircleCastAll(transform.position, BUBBLE_SPACING * sizeMultiplier, Vector2.up, 0.0f))
        {
            if (hit.collider.gameObject.tag == "Bubble")
            {
                hit.collider.gameObject.transform.position = new Vector3(-1000.0f, -1000.0f);
                var attachment = hit.collider.gameObject.GetComponent<BubbleAttachments>();
                attachment.Model.DisconnectAll();
                attachment.MarkForDestruction();
            }
        }
    }
}
