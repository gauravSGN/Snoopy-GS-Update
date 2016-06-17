using UnityEngine;

public class BubbleDefinition : ScriptableObject, GameObjectDefinition<BubbleType>
{
    public BubbleType Type { get { return type; } }
    public GameObject Prefab { get { return prefab; } }
    public Color BaseColor { get { return baseColor; } }
    public bool ActsAsRoot { get { return actsAsRoot; } }
    public int Score { get { return score; } }

    [SerializeField]
    private BubbleType type;

    [SerializeField]
    private Color baseColor;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private bool actsAsRoot;

    [SerializeField]
    private int score;
}
