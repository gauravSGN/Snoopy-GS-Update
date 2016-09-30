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
        public float cullMinDistance;
        public float cullMaxDistance;
    }

    [Serializable]
    public class BubblePartyConfig
    {
        public float minXForce;
        public float maxXForce;
        public float minYForce;
        public float maxYForce;
        public float delayBetweenBubbles;
    }

    [Serializable]
    public class WinSequenceConfig
    {
        public float delayBeforeCullAll;
        public float delayBeforeWinTextAnimation;
        public float delayBeforeCelebration;
        public float delayBeforeBubbleParty;
        public float delayBeforeSlideOut;
        public float delayBeforePopup;
    }

    [Serializable]
    public class ImpactEffectConfig
    {
        public float duration;
        public float distance;
        public float radius;

        public AnimationCurve easing;

        public AnimationCurve distanceScaling;
    }

    [Serializable]
    public class AimlineConfig
    {
        public float length;
        public float lineWidth;
        public float dotSpacing;
        public float moveSpeed;
        public float launchSpeed;

        public AimLine.Settings normal;
        public AimLine.Settings extended;

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
        public float multiplierCalloutDelay;
        public float multiplierCalloutFadeTime;
        public GoalScores[] goals;
    }

    [Serializable]
    public class BoosterConfig
    {
        public Color[] rainbowColors;
    }

    [Serializable]
    public class LevelConfig
    {
        public int maxLevel;
        public int lowMovesThreshold;
        public float levelLostDelay;
    }

    [Serializable]
    public class WoodstockConfig
    {
        public float majorLandingRadius;
        public float minorLandingRadius;
        public float landingSpread;
        public float flightSpeed;
        public float bumpScale;
        public float bumpDuration;
    }

    [Serializable]
    public class PowerUpConfig
    {
        public float popOrderDelay;
    }

    [Serializable]
    public class BossConfig
    {
        public float flightSpeed;
        public float bankAngle;
    }

    public BubbleConfig bubbles;
    public ReactionConfig reactions;
    public ImpactEffectConfig impactEffect;
    public AimlineConfig aimline;
    public PurchasableConfig purchasables;
    public ScoringConfig scoring;
    public BoosterConfig boosters;
    public LevelConfig level;
    public WoodstockConfig woodstock;
    public BubblePartyConfig bubbleParty;
    public WinSequenceConfig winSequence;
    public PowerUpConfig powerUp;
    public BossConfig boss;
}
