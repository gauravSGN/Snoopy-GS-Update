using UnityEngine;
using System.Collections;

public class BubbleLauncher : MonoBehaviour
{
    public BubbleFactory factory;
    public GameObject launchOrigin;
    public float launchSpeed;

    private GameObject nextBubble;

    protected void Start()
    {
        nextBubble = CreateNextBubble();
    }

    protected void OnMouseUp()
    {
        if (nextBubble != null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit != null)
            {
                FireBubbleAt(hit.point);

                nextBubble = null;
                StartCoroutine(ReadyNextBubble());
            }
        }
    }

    private GameObject CreateNextBubble()
    {
        var instance = factory.CreateBubbleByType((BubbleType)Random.Range(0, 4));

        instance.transform.position = launchOrigin.transform.position;
        instance.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        return instance;
    }

    private void FireBubbleAt(Vector2 point)
    {
        var direction = (point - (Vector2)launchOrigin.transform.position).normalized * launchSpeed;
        var rigidBody = nextBubble.GetComponent<Rigidbody2D>();

        nextBubble.AddComponent<BubbleSnap>();
        rigidBody.velocity = direction;
    }

    private IEnumerator ReadyNextBubble()
    {
        yield return new WaitForSeconds(0.5f);

        nextBubble = CreateNextBubble();
    }
}
