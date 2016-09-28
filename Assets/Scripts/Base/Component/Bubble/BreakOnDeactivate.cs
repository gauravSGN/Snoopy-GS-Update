using Util;
using UnityEngine;

sealed public class BreakOnDeactivate : MonoBehaviour
{
    public void Start()
    {
        var model = GetComponent<BubbleModelBehaviour>().Model;
        model.OnActivationChanged += OnActivationChanged;

        GlobalState.EventService.AddEventHandler<BubbleSettledEvent>(OnSettled);
    }

    private void OnSettled()
    {
        GetComponent<BubbleModelBehaviour>().Model.OnActivationChanged -= OnActivationChanged;
        GlobalState.EventService.RemoveEventHandler<BubbleSettledEvent>(OnSettled);
    }

    private void OnActivationChanged(Bubble model, bool active)
    {
        if (!active)
        {
            model.OnActivationChanged -= OnActivationChanged;
            MoveToValidLocation();

            model.Active = true;
            GetComponent<BubbleSnap>().CompleteSnap();

            Destroy(this);
        }
    }

    private void MoveToValidLocation()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        var velocity = (Vector3)rigidBody.velocity;
        var position = transform.position;

        position -= velocity * Time.deltaTime;

        var config = GlobalState.Instance.Config;
        var rowDistance = config.bubbles.size * MathUtil.COS_30_DEGREES;
        var topEdge = Camera.main.orthographicSize - (0.5f * rowDistance);
        var leftEdge = -(config.bubbles.numPerRow - 1) * config.bubbles.size / 2.0f;

        var y = Mathf.RoundToInt((topEdge - position.y) / rowDistance);
        var offset = (y & 1) * config.bubbles.size / 2.0f;
        var x = Mathf.RoundToInt((position.x - leftEdge - offset) / config.bubbles.size);

        transform.position = new Vector3(leftEdge + x * config.bubbles.size + offset, topEdge - y * rowDistance);
    }
}
