using UnityEngine;

public class BubbleAttachments : MonoBehaviour
{
    public Bubble Model { get { return model; } }

    public AnimatorOverrideController animationController;

    [SerializeField]
    private GameObject animationObject;

    [SerializeField]
    private Bubble model;

    public void Attach(GameObject other)
    {
        Model.Connect(other.GetComponent<BubbleAttachments>().Model);
    }

    public void SetModel(Bubble model)
    {
        this.model = model;

        model.OnPopped += PoppedHandler;
        model.OnDisconnected += DisconnectedHandler;
    }

    private void RemoveHandlers()
    {
        Model.OnPopped -= PoppedHandler;
        Model.OnDisconnected -= DisconnectedHandler;
    }

    private void PoppedHandler(Bubble bubble)
    {
        StartDeathAnimation();
        RemoveHandlers();
        Destroy(gameObject);
    }

    private void DisconnectedHandler(Bubble bubble)
    {
        RemoveHandlers();

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = (int)Layers.FallingObjects;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(Random.Range(-3.0f, 3.0f), 0.0f));
    }

    private void StartDeathAnimation()
    {
        // var child = gameObject.transform.GetComponentInChildren<SpriteRenderer>().gameObject;
        // child.AddComponent<Animator>();
        GameObject container = (GameObject)Instantiate(animationObject, gameObject.transform.position, Quaternion.identity);
        var animator = container.GetComponent<Animator>();
        animator.runtimeAnimatorController = animationController;
    }
}
