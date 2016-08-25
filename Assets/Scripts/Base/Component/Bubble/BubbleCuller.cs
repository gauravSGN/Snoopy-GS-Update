using UnityEngine;
using System.Collections;

sealed public class BubbleCuller : MonoBehaviour
{
    private Bubble model;
    private Rigidbody2D rigidBody;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetModel(Bubble model)
    {
        this.model = model;

        model.OnDisconnected += OnDisconnected;
    }

    public void OnDestroy()
    {
        RemoveListeners();
    }

    private void RemoveListeners()
    {
        model.OnDisconnected -= OnDisconnected;
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
