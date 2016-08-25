using UnityEngine;
using System.Collections;

sealed public class BubbleCuller : BubbleModelBehaviour
{
    private Rigidbody2D rigidBody;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    override protected void AddListeners()
    {
        Model.OnDisconnected += OnDisconnected;
    }

    override protected void RemoveListeners()
    {
        Model.OnDisconnected -= OnDisconnected;
    }

    private void OnDisconnected(Bubble bubble)
    {
        RemoveListeners();

        var config = GlobalState.Instance.Config.reactions;

        gameObject.layer = (int)Layers.FallingObjects;

        var xForce = Random.Range(config.cullMinXForce, config.cullMaxXForce);
        var yForce = Random.Range(config.cullMinYForce, config.cullMaxYForce);
        var force = new Vector2(xForce, yForce);

        rigidBody.isKinematic = false;
        rigidBody.AddForce(force);

        var distance = Random.Range(config.cullMinDistance, config.cullMaxDistance);
        var acceleration = Physics2D.gravity + force / (rigidBody.mass / Time.fixedDeltaTime);
        var timeToLive = Mathf.Sqrt(distance / Mathf.Sqrt(acceleration.sqrMagnitude / 4.0f));

        StartCoroutine(PopAfterSeconds(timeToLive));
    }

    private IEnumerator PopAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Cull);
    }
}
