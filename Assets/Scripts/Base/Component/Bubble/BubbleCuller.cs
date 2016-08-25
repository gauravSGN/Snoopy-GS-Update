using UnityEngine;
using System.Collections.Generic;

sealed public class BubbleCuller : MonoBehaviour
{
    private Bubble model;

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

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = (int)Layers.FallingObjects;

        var xForce = Random.Range(config.cullMinXForce, config.cullMaxXForce);
        var yForce = Random.Range(config.cullMinYForce, config.cullMaxYForce);

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(xForce, yForce));

        var distance = Random.Range(config.cullMinDistance, config.cullMaxDistance);
        var gravity = Physics2D.gravity;
    }
}
