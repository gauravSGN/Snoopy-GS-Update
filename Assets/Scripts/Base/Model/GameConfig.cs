using UnityEngine;
using System;

public class GameConfig : ScriptableObject
{
    [Serializable]
    public class BubbleConfig
    {
        public int numPerRow;
        public float size;
        public float shotColliderScale;
    }

    [Serializable]
    public class ReactionConfig
    {
        public float popSpreadDelay;
        public float cullMinXForce;
        public float cullMaxXForce;
        public float cullMinYForce;
        public float cullMaxYForce;
    }

    [Serializable]
    public class ImpactEffectConfig
    {
        public float duration;
        public float distance;
        public float radius;

        public AnimationCurve easing;
    }

    [Serializable]
    public class AimlineConfig
    {
        public float length;
        public float lineWidth;
        public float dotSpacing;
        public float moveSpeed;
        public float wallBounceDistance;

        public float colliderAdjustment;
    }

    [Serializable]
    public class PurchasableConfig
    {
        public int maxHearts;
        public int secondsPerHeart;
        public int newUserCoins;
    }

    public BubbleConfig bubbles;
    public ReactionConfig reactions;
    public ImpactEffectConfig impactEffect;
    public AimlineConfig aimline;
    public PurchasableConfig purchasables;
}
