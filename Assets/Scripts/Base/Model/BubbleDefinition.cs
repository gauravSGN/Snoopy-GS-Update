using Model;
using UnityEngine;
using Animation;
using System.Collections.Generic;
using System;

public class BubbleDefinition : ScriptableObject, GameObjectDefinition<BubbleType>
{
    public BubbleType Type { get { return type; } }
    public BubbleColor Color { get { return color; } }
    public GameObject Prefab { get { return prefab; } }
    public Color BaseColor { get { return baseColor; } }
    public bool ActsAsRoot { get { return actsAsRoot; } }
    public bool Activatible { get { return activatible; } }
    public int Score { get { return score; } }
    public int MatchThreshold { get { return matchThreshold; } }

    public Dictionary<BubbleDeathType, List<AnimationType>> AnimationMap
    {
        get
        {
            if (animationMap == null)
            {
                animationMap = new Dictionary<BubbleDeathType, List<AnimationType>>();

                foreach (var item in defaultAnimations)
                {
                    animationMap[item.name] = item.list;
                }
            }

            return animationMap;
        }
    }

    [Serializable]
    public struct NamedAnimationList
    {
        public BubbleDeathType name;
        public List<AnimationType> list;
    }

    [SerializeField]
    public BubbleCategory category;

    [SerializeField]
    private BubbleType type;

    [SerializeField]
    private BubbleColor color;

    [SerializeField]
    private Color baseColor;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private bool actsAsRoot;

    [SerializeField]
    private bool activatible;

    [SerializeField]
    private int score;

    [SerializeField]
    private int matchThreshold;

    [SerializeField]
    private List<NamedAnimationList> defaultAnimations;

    private Dictionary<BubbleDeathType, List<AnimationType>> animationMap;
}
