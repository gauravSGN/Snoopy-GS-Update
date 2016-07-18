using UnityEngine;
using System.Collections.Generic;
using Goal;
using PowerUps;
using Util;
using Model;
using System.Linq;
using Service;

public class LevelLoader : MonoBehaviour
{
    public LevelData LevelData { get; private set; }
    public BubbleQueueType BubbleQueueType { get { return bubbleQueueType; } }

    [SerializeField]
    private BubbleFactory bubbleFactory;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private PowerUpController powerUpController;

    [SerializeField]
    private BubbleQueueType bubbleQueueType;

    [SerializeField]
    private GameObject levelContainer;

    private float rowDistance;
    private float topEdge;

    public Dictionary<BubbleType, int> LoadLevel(string levelData)
    {
        rowDistance = GlobalState.Instance.Config.bubbles.size * MathUtil.COS_30_DEGREES;
        Dictionary<BubbleType, int> bubbleTypeCount;

        LevelData = JsonUtility.FromJson<LevelData>(levelData);
        bubbleTypeCount = CreateLevel(LevelData);
        powerUpController.Setup(LevelData.PowerUpFills);
        PositionCamera();

        return bubbleTypeCount;
    }

    private void PositionCamera()
    {
        var scroll = gameView.GetComponent<LevelIntroScroll>();
        var maxY = LevelData.Bubbles.Aggregate(1, (acc, b) => Mathf.Max(acc, b.Y));
        var targetY = -(maxY - 11) * GlobalState.Instance.Config.bubbles.size * MathUtil.COS_30_DEGREES;
        scroll.ScrollTo(targetY);
    }

    private Vector3 GetBubbleLocation(int x, int y)
    {
        var config = GlobalState.Instance.Config;
        var offset = (y & 1) * config.bubbles.size / 2.0f;
        var leftEdge = -(config.bubbles.numPerRow - 1) * config.bubbles.size / 2.0f;
        return new Vector3(leftEdge + x * config.bubbles.size + offset, topEdge - y * rowDistance);
    }

    private Dictionary<BubbleType, int> CreateLevel(LevelData level)
    {
        var bubbleMap = new Dictionary<int, GameObject>();

        topEdge = Camera.main.orthographicSize - (0.5f * rowDistance);

        var bubbleTypeCount = new Dictionary<BubbleType, int>();

        foreach (var bubble in level.Bubbles)
        {
            var bubbleType = BubbleType.IsDefined(typeof(BubbleType), bubble.Type) ? bubble.Type : (BubbleType)((int)bubble.Type % 6);

            bubbleTypeCount[bubbleType] = bubbleTypeCount.ContainsKey(bubbleType) ? bubbleTypeCount[bubbleType] + 1 : 1;
            bubbleMap[bubble.Key] = createBubbleAndSetPosition(bubble);
            bubble.model = bubbleMap[bubble.Key].GetComponent<BubbleAttachments>().Model;
        }

        AttachBubbles(bubbleMap);
        ComputeRootDistances(bubbleMap);

        foreach (var pair in bubbleMap)
        {
            pair.Value.GetComponent<BubbleAttachments>().Model.SortNeighbors();
        }

        CreateGoals(level);

        return bubbleTypeCount;
    }

    private static void CreateGoals(LevelData level)
    {
        level.goals = LevelGoalFactory.GetGoalsForLevel(level);

        foreach (var goal in level.goals)
        {
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new GoalCreatedEvent(goal));
        }
    }

    private void AttachBubbles(Dictionary<int, GameObject> bubbleMap)
    {
        var neighbors = new int[6];
        var ceilingBubbleCount = GlobalState.Instance.Config.bubbles.numPerRow + 1;
        var ceilingBubbleMap = new Dictionary<int, GameObject>();

        for (int ceilingX = 0; ceilingX < ceilingBubbleCount; ceilingX++)
        {
            var ceilingData = new BubbleData(ceilingX - 1, -1, BubbleType.Ceiling);
            ceilingBubbleMap[ceilingX - 1] = createBubbleAndSetPosition(ceilingData);
        }

        foreach (var pair in bubbleMap)
        {
            int x = pair.Key & 0xf;
            int y = pair.Key >> 4;
            var attachments = pair.Value.GetComponent<BubbleAttachments>();

            GetNeighbors(x, y, neighbors);

            foreach (var neighbor in neighbors)
            {
                if (neighbor >= 0 && bubbleMap.ContainsKey(neighbor))
                {
                    attachments.Attach(bubbleMap[neighbor]);
                }
            }

            if (y == 0)
            {
                attachments.Attach(ceilingBubbleMap[x - 1]);
                attachments.Attach(ceilingBubbleMap[x]);
            }
        }

        foreach (var pair in ceilingBubbleMap)
        {
            var model = pair.Value.GetComponent<BubbleAttachments>().Model;
            model.DistanceFromRoot = 0;
            model.PropagateRootDistance();
        }
    }

    private void ComputeRootDistances(Dictionary<int, GameObject> bubbleMap)
    {
        foreach (var pair in bubbleMap)
        {
            var model = pair.Value.GetComponent<BubbleAttachments>().Model;

            if (model.IsRoot)
            {
                model.DistanceFromRoot = 0;
                model.PropagateRootDistance();
            }
        }
    }

    private GameObject createBubbleAndSetPosition(BubbleData bubbleData)
    {
        var instance = bubbleFactory.Create(bubbleData);

        if (levelContainer != null)
        {
            instance.transform.SetParent(levelContainer.transform, false);
        }

        instance.transform.localPosition = GetBubbleLocation(bubbleData.X, bubbleData.Y);

        return instance;
    }

    private void GetNeighbors(int x, int y, int[] neighbors)
    {
        var offset = (y & 1) * 2 - 1;

        neighbors[0] = BubbleData.GetKey(x + offset, y - 1);
        neighbors[1] = BubbleData.GetKey(x, y - 1);
        neighbors[2] = BubbleData.GetKey(x - 1, y);
        neighbors[3] = BubbleData.GetKey(x + 1, y);
        neighbors[4] = BubbleData.GetKey(x + offset, y + 1);
        neighbors[5] = BubbleData.GetKey(x, y + 1);
    }
}
