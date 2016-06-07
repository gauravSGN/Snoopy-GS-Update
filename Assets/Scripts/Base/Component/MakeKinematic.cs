using UnityEngine;

public class MakeKinematic : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    void Awake()
    {
        body.isKinematic = true;
    }
}
