using UnityEngine;
using System.Collections;

public class BubbleCuller : BubbleModelBehaviour
{
    override protected void AddListeners()
    {
        Model.OnDisconnected += OnDisconnected;
    }

    override protected void RemoveListeners()
    {
        Model.OnDisconnected -= OnDisconnected;
    }

    virtual protected void CullBubble(Bubble bubble)
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        var config = GlobalState.Instance.Config.reactions;
        var xForce = Random.Range(config.cullMinXForce, config.cullMaxXForce);
        var yForce = Random.Range(config.cullMinYForce, config.cullMaxYForce);

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(xForce, yForce));

        StartCoroutine(CullAfterFallingDistance(Random.Range(config.cullMinDistance, config.cullMaxDistance)));
    }

    private void OnDisconnected(Bubble bubble)
    {
        RemoveListeners();

        gameObject.layer = (int)Layers.FallingObjects;

        CullBubble(bubble);
    }

    private IEnumerator CullAfterFallingDistance(float distance)
    {
        var total = 0.0f;
        var myTransform = transform;
        var lastPosition = myTransform.localPosition;

        while (total < distance)
        {
            var position = myTransform.localPosition;

            if (position.y < lastPosition.y)
            {
                gameObject.layer = (int)Layers.FallingObjects;
                total += (position - lastPosition).magnitude;
            }

            lastPosition = position;
            yield return null;
        }

        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
