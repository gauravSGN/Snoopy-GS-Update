using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelData;
    public BubbleFactory factory;
    public GameConfig config;

    private const float COS_30_DEGREES = 0.8660254038f;

    protected void Start()
    {
        using (var reader = new StringReader(levelData.text))
        {
            CreateLevel(ParseLevelData(reader));
        }
    }

    private LevelData ParseLevelData(StringReader reader)
    {
        var serializer = new XmlSerializer(typeof(LevelData));
        return (LevelData)serializer.Deserialize(reader);
    }

    private Vector3 GetBubbleLocation(int x, int y)
    {
        var offset = (1 + (y & 1)) * config.bubbleSize / 2.0f;
        var leftEdge = -config.bubblesPerRow * config.bubbleSize / 2.0f;
        var topEdge = Camera.main.orthographicSize + config.bubbleSize * COS_30_DEGREES / 2.0f;
        return new Vector3(leftEdge + x * config.bubbleSize + offset, topEdge - y * config.bubbleSize * COS_30_DEGREES);
    }

    private void CreateLevel(LevelData level)
    {
        var bubbleMap = new Dictionary<int, GameObject>();

        foreach (var bubble in level.bubbles)
        {
            var instance = factory.CreateBubbleByType((BubbleType)(bubble.typeID % 4));
            instance.transform.position = GetBubbleLocation(bubble.x, bubble.y);
            bubbleMap[bubble.y << 4 | bubble.x] = instance;
        }

        AttachBubbles(bubbleMap);
    }

    private void AttachBubbles(Dictionary<int, GameObject> bubbleMap)
    {
        var neighbors = new int[6];

        foreach (var pair in bubbleMap)
        {
            int x = pair.Key & 0xf;
            int y = pair.Key >> 4;

            if (y == 1)
            {
                var anchor = pair.Value.AddComponent<FixedJoint2D>();
            }

            GetNeighbors(x, y, neighbors);

            foreach (var neighbor in neighbors)
            {
                if (neighbor >= 0)
                {
                    if (bubbleMap.ContainsKey(neighbor))
                    {
                        pair.Value.GetComponent<BubbleAttachments>().Attach(bubbleMap[neighbor]);
                    }
                }
            }
        }
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
