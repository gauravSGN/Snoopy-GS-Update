using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleLauncher : MonoBehaviour
{
    public GameObject[] locations;
    public float launchSpeed;
    public Level level;

    private GameObject[] nextBubbles;

    public void CycleQueue()
    {
        CycleLocalQueue();
        level.LevelState.bubbleQueue.RotateQueue(nextBubbles.Length);
    }

    protected void Start()
    {
        nextBubbles = new GameObject[locations.Length];

        CreateBubbles();
    }

    protected void OnMouseUp()
    {
        if (nextBubbles[0] != null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                FireBubbleAt(hit.point);
                StartCoroutine(ReadyNextBubble());
            }
        }
    }

    private void CreateBubbles()
    {
        for (var index = 0; index < nextBubbles.Length; index++)
        {
            if (nextBubbles[index] == null)
            {
                nextBubbles[index] = level.bubbleFactory.CreateBubbleByType(level.LevelState.bubbleQueue.Peek(index));
                MoveBubbleToLocation(index);
            }
        }
    }

    private void FireBubbleAt(Vector2 point)
    {
        var direction = (point - (Vector2)locations[0].transform.position).normalized * launchSpeed;
        var rigidBody = nextBubbles[0].GetComponent<Rigidbody2D>();

        nextBubbles[0].transform.parent = null;
        nextBubbles[0].AddComponent<BubbleSnap>();

        rigidBody.isKinematic = false;
        rigidBody.velocity = direction;
        rigidBody.gravityScale = 0.0f;

        EventDispatcher.Instance.Dispatch(new BubbleFiredEvent());

        nextBubbles[0] = null;
    }

    private IEnumerator ReadyNextBubble()
    {
        yield return new WaitForSeconds(0.5f);

        level.LevelState.bubbleQueue.GetNext();

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
    }
}
