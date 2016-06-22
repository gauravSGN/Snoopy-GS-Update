using UnityEngine;
using System.Collections.Generic;
using Goal;
using BubbleContent;
using PowerUps;
using Util;
using Model;

public class LevelLoader : MonoBehaviour
{
    public LevelData LevelData { get; private set; }

    [SerializeField]
    private BubbleFactory bubbleFactory;

    [SerializeField]
    private BubbleContentFactory contentFactory;

    [SerializeField]
    private GameConfig config;

    [SerializeField]
    private GameObject gameView;

    [SerializeField]
    private PowerUpController powerUpController;

    [SerializeField]
    private GameObject levelContainer;

    [SerializeField]
    private List<Lantern> lanterns;

    private float rowDistance;
    private float topEdge;

    public Dictionary<BubbleType, int> LoadLevel(string levelData)
    {
        rowDistance = config.bubbles.size * MathUtil.COS_30_DEGREES;
        Dictionary<BubbleType, int> bubbleTypeCount;

        LevelData = JsonUtility.FromJson<LevelData>(levelData);
        bubbleTypeCount = CreateLevel(LevelData);
        SetupPowerUps();

        return bubbleTypeCount;
    }

    private void SetupPowerUps()
    {
        var count = Mathf.Min(lanterns.Count, LevelData.PowerUpFills.Length);
        for (var index = 0; index < count; index++)
        {
            float lanternFillValue = LevelData.PowerUpFills[index];
            lanterns[index].Setup((int)(lanternFillValue > 0.0f ? (1.0 / lanternFillValue) : 0.0f));
        }

        powerUpController.Setup(levelData);
    }

    private Vector3 GetBubbleLocation(int x, int y)
    {
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
            var bubbleType = (BubbleType)((int)bubble.Type % 6);
            bubbleTypeCount[bubbleType] = bubbleTypeCount.ContainsKey(bubbleType) ? bubbleTypeCount[bubbleType] + 1 : 1;
            bubbleMap[bubble.Key] = createBubbleAndSetPosition((BubbleType)((int)bubble.Type % 6), bubble.X, bubble.Y);
            bubble.model = bubbleMap[bubble.Key].GetComponent<BubbleAttachments>().Model;

            if (bubble.ContentType != BubbleContentType.None)
            {
                var content = contentFactory.CreateByType((BubbleContentType)bubble.contentType);

                if (content != null)
                {
                    content.transform.SetParent(bubbleMap[bubble.Key].transform, false);
                    content.transform.localPosition = Vector3.back;
                }
            }
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
            GlobalState.Instance.EventDispatcher.Dispatch(new GoalCreatedEvent(goal));
        }
    }

    private void AttachBubbles(Dictionary<int, GameObject> bubbleMap)
    {
        var neighbors = new int[6];
        var maxBubblesForOddRow = config.bubbles.numPerRow - 1;
        var ceilingBubbleMap = new Dictionary<int, GameObject>();

        for (int ceilingX = 0; ceilingX < maxBubblesForOddRow; ceilingX++)
        {
            ceilingBubbleMap[ceilingX] = createBubbleAndSetPosition(BubbleType.Ceiling, ceilingX, 0);
        }

        foreach (var pair in bubbleMap)
        {
            int x = pair.Key & 0xf;
            int y = pair.Key >> 4;

            GetNeighbors(x, y, neighbors);

            foreach (var neighbor in neighbors)
            {
                if (neighbor >= 0 && bubbleMap.ContainsKey(neighbor))
                {
                    pair.Value.GetComponent<BubbleAttachments>().Attach(bubbleMap[neighbor]);
                }
            }

            if (y == 1)
            {
                pair.Value.GetComponent<BubbleAttachments>().Attach(ceilingBubbleMap [Mathf.Min(x, maxBubblesForOddRow - 1)]);
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

    private GameObject createBubbleAndSetPosition(BubbleType type, int x, int y)
    {
        var instance = bubbleFactory.CreateByType(type);

        if (levelContainer != null)
        {
            instance.transform.SetParent(levelContainer.transform, false);
        }

        instance.transform.localPosition = GetBubbleLocation(x, y);

        return instance;
    }

    private void GetNeighbors(int x, int y, int[] neighbors)
    {
        var offset = 1 - (y & 1) * 2;

        neighbors[0] = LevelData.BubbleData.GetKey(x + offset, y - 1);
        neighbors[1] = LevelData.BubbleData.GetKey(x, y - 1);
        neighbors[2] = LevelData.BubbleData.GetKey(x - 1, y);
        neighbors[3] = LevelData.BubbleData.GetKey(x + 1, y);
        neighbors[4] = LevelData.BubbleData.GetKey(x + offset, y + 1);
        neighbors[5] = LevelData.BubbleData.GetKey(x, y + 1);
    }
}
