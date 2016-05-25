using UnityEngine;
using System.IO;
using System.Collections;
using System.Xml.Serialization;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelData;
    public BubbleFactory factory;
    public GameConfig config;

    protected void Start()
    {
        using (var reader = new StringReader(levelData.text))
        {
            var level = ParseLevelData(reader);

            foreach (var bubble in level.bubbles)
            {
                var instance = factory.CreateBubbleByType((BubbleType)bubble.typeID);
                instance.transform.position = GetBubbleLocation(bubble.x, bubble.y);
            }
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
        var topEdge = Camera.main.orthographicSize + config.bubbleSize / 2.0f;
        return new Vector3(leftEdge + x * config.bubbleSize + offset, topEdge - y * config.bubbleSize);
    }
}
