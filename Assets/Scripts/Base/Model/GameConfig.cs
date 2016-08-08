using Goal;
using System;
using UnityEngine;

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

        public int maxReflections;
        public int maxExtendedReflections;
        public float reflectionDistance;
        public float extendedReflectionDistance;

        public float colliderAdjustment;
    }

    [Serializable]
    public class PurchasableConfig
    {
        public int maxHearts;
        public int secondsPerHeart;
        public int newUserCoins;
    }

    [Serializable]
    public class ScoringConfig
    {
        [Serializable]
        public struct GoalScores
        {
            public GoalType goalType;
            public int score;
        }

        public AnimationCurve rollup;
        public float rollupDuration;
        public float dropMultiplier;
        public int minClusterSize;
        public int maxClusterSize;
        public float clusterCoefficient;
        public int remainingMovesValue;
        public float popToDropFactor;
        public float secondStarDivisor;
        public GoalScores[] goals;
    }

    [Serializable]
    public class BoosterConfig
    {
        public Color[] rainbowColors;
    }

    public BubbleConfig bubbles;
    public ReactionConfig reactions;
    public ImpactEffectConfig impactEffect;
    public AimlineConfig aimline;
    public PurchasableConfig purchasables;
    public ScoringConfig scoring;
    public BoosterConfig boosters;
}
