using UnityEngine;

public class BubbleDefinition : ScriptableObject, GameObjectDefinition<BubbleType>
{
    public BubbleType Type { get { return type; } }

    public BubbleType type;
    public Color baseColor;
    public GameObject prefab;
    public bool actsAsRoot;
    public int score;
}
