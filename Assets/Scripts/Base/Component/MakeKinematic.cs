using UnityEngine;

public class MakeKinematic : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    protected void Awake()
    {
        body.isKinematic = true;
    }
}
