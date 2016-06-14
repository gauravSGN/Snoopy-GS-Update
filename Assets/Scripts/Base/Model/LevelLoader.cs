using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using Goal;
using BubbleContent;

public class LevelLoader : MonoBehaviour
{
    private const float COS_30_DEGREES = 0.8660254038f;

    public LevelData LevelData { get; private set; }

    [SerializeField]
    private BubbleFactory factory;

    [SerializeField]
    private GameConfig config;

    [SerializeField]
    private GameObject gameView;

    private float rowDistance;
    private float topEdge;
    private int maxY;

    public Dictionary<BubbleType, int> LoadLevel(TextAsset levelData)
    {
        rowDistance = config.bubbles.size * COS_30_DEGREES;
        Dictionary<BubbleType, int> bubbleTypeCount;

        using (var reader = new StringReader(levelData.text))
        {
            LevelData = ParseLevelData(reader);
            bubbleTypeCount = CreateLevel(LevelData);
            SetupLanterns();
        }

        return bubbleTypeCount;
    }

    private void SetupLanterns()
    {
        var lanterns = new Dictionary<string, string>();
        lanterns["bombFill"] = "Red Lantern";
        lanterns["horzFill"] = "Blue Lantern";
        lanterns["snakeFill"] = "Green Lantern";
        lanterns["fireFill"] = "Yellow Lantern";

        foreach (var entry in lanterns)
        {
            var lantern = gameView.transform.FindChild("Lanterns").FindChild(entry.Value).gameObject;
            float lanternFillValue = (float)LevelData.GetType().GetField(entry.Key).GetValue(LevelData);

            lantern.GetComponent<Lantern>().Setup((int)(lanternFillValue > 0.0f ? (1.0 / lanternFillValue) : 0.0f));
        }
    }

    private LevelData ParseLevelData(StringReader reader)
    {
        var serializer = new XmlSerializer(typeof(LevelData));
        return (LevelData)serializer.Deserialize(reader);
    }

    private Vector3 GetBubbleLocation(int x, int y)
    {
        var offset = (2 - ((maxY + y) & 1)) * config.bubbles.size / 2.0f;
        var leftEdge = -config.bubbles.numPerRow * config.bubbles.size / 2.0f;
        return new Vector3(leftEdge + x * config.bubbles.size + offset, topEdge - y * rowDistance);
    }

    private Dictionary<BubbleType, int> CreateLevel(LevelData level)
    {
        var bubbleMap = new Dictionary<int, GameObject>();
        maxY = 0;

        foreach (var bubble in level.bubbles)
        {
            maxY = Mathf.Max(maxY, bubble.y);
        }

        gameView.transform.position = new Vector3(0.0f, -rowDistance * Mathf.Max(0.0f, maxY - 8));
        topEdge = Camera.main.orthographicSize + (0.5f * rowDistance);

        var bubbleTypeCount = new Dictionary<BubbleType, int>();

        foreach (var bubble in level.bubbles)
        {
            bubble.y = (maxY + 1) - bubble.y;
            var bubbleType = (BubbleType)(bubble.typeID % 6);
            bubbleTypeCount[bubbleType] = bubbleTypeCount.ContainsKey(bubbleType) ? bubbleTypeCount[bubbleType] + 1 : 1;
            bubbleMap[bubble.y << 4 | bubble.x] = createBubbleAndSetPosition((BubbleType)(bubble.typeID % 6), bubble.x, bubble.y);
            bubble.model = bubbleMap[bubble.y << 4 | bubble.x].GetComponent<BubbleAttachments>().Model;

            if ((BubbleContentType)bubble.contentType != BubbleContentType.None)
            {
                var content = factory.CreateContentByType((BubbleContentType)bubble.contentType);

                if (content != null)
                {
                    content.transform.parent = bubbleMap[bubble.y << 4 | bubble.x].transform;
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
        var maxBubblesForOddRow = config.bubbles.numPerRow - ((maxY & 1) ^ 1);
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
        var instance = factory.CreateBubbleByType(type);

        instance.transform.position = GetBubbleLocation(x, y);

        return instance;
    }

    private void GetNeighbors(int x, int y, int[] neighbors)
    {
        var offset = 1 - ((maxY + y) & 1) * 2;

        neighbors[0] = ((y - 1) << 4) | (x + offset);
        neighbors[1] = ((y - 1) << 4) | x;
        neighbors[2] = (y << 4) | (x - 1);
        neighbors[3] = (y << 4) | (x + 1);
        neighbors[4] = ((y + 1) << 4) | (x + offset);
        neighbors[5] = ((y + 1) << 4) | x;
    }
}
