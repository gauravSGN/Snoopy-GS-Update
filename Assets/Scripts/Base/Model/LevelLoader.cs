using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

public class LevelLoader : MonoBehaviour
{
    public LevelData LevelData { get; private set; }

    public BubbleFactory factory;
    public GameConfig config;
    public GameObject gameView;

    private const float COS_30_DEGREES = 0.8660254038f;
    private float rowDistance;
    private float topEdge;
    private int maxY;

    public void LoadLevel(TextAsset levelData)
    {
        rowDistance = config.bubbles.size * COS_30_DEGREES;

        using (var reader = new StringReader(levelData.text))
        {
            LevelData = ParseLevelData(reader);
            CreateLevel(LevelData);
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

    private void CreateLevel(LevelData level)
    {
        var bubbleMap = new Dictionary<int, GameObject>();
        maxY = 0;

        foreach (var bubble in level.bubbles)
        {
            maxY = Mathf.Max(maxY, bubble.y);
        }

        gameView.transform.position = new Vector3(0.0f, -rowDistance * Mathf.Max(0.0f, maxY - 8));
        topEdge = Camera.main.orthographicSize + (0.5f * rowDistance);

        foreach (var bubble in level.bubbles)
        {
            bubble.y = (maxY + 1) - bubble.y;
            bubbleMap[bubble.y << 4 | bubble.x] = createBubbleAndSetPosition((BubbleType)(bubble.typeID % 4), bubble.x, bubble.y);
        }

        AttachBubbles(bubbleMap);
    }

    private void AttachBubbles(Dictionary<int, GameObject> bubbleMap)
    {
        var neighbors = new int[6];
        var maxBubblesForOddRow = config.bubbles.numPerRow - ((maxY & 1) ^ 1);
        var ceilingBubbleMap = new Dictionary<int, GameObject>();

        for (int ceilingX = 0; ceilingX < maxBubblesForOddRow; ceilingX++)
        {
            ceilingBubbleMap[ceilingX] = createBubbleAndSetPosition(BubbleType.Ceiling, ceilingX, 0, true);
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
    }

    private GameObject createBubbleAndSetPosition(BubbleType type, int x, int y, bool isRoot = false)
    {
        var instance = factory.CreateBubbleByType(type);

        instance.transform.position = GetBubbleLocation(x, y);
        instance.GetComponent<BubbleAttachments>().Model.IsRoot = isRoot;

        return instance;
    }

    private void GetNeighbors(int x, int y, int[] neighbors)
    {
        var offset = (y & 1) * 2 - 1;

        neighbors[0] = ((y - 1) << 4) | (x + offset);
        neighbors[1] = ((y - 1) << 4) | x;
        neighbors[2] = (y << 4) | (x - 1);
        neighbors[3] = (y << 4) | (x + 1);
        neighbors[4] = ((y + 1) << 4) | (x + offset);
        neighbors[5] = ((y + 1) << 4) | x;
    }
}
