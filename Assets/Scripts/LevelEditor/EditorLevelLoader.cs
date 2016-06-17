using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LevelEditor
{
    public class EditorLevelLoader : MonoBehaviour
    {
        private const int BUBBLE_SIZE = 32;

        [SerializeField]
        private RectTransform bubbleContainer;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private BubbleFactory factory;

        private int maxY;

        public void Clear()
        {
            for (var index = bubbleContainer.childCount - 1; index >= 0; index--)
            {
                Destroy(bubbleContainer.GetChild(index).gameObject);
            }
        }

        public void LoadLevel(string xmlText)
        {
            var levelData = XmlUtil.Deserialize<LevelData>(xmlText);
            maxY = levelData.bubbles.Aggregate(1, (acc, b) => Mathf.Max(acc, b.y));

            foreach (var bubble in levelData.bubbles)
            {
                CreateBubble(bubble);
            }
        }

        private void CreateBubble(LevelData.BubbleData bubble)
        {
            var definition = factory.GetBubbleDefinitionByType((BubbleType)bubble.typeID);
            var prefabRenderer = definition.prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabRenderer != null)
            {
                var instance = Instantiate(prefab);

                var image = instance.GetComponent<Image>();
                image.sprite = prefabRenderer.sprite;

                instance.transform.SetParent(bubbleContainer, false);
                instance.transform.localPosition = GetBubbleLocation(bubble.x, bubble.y);
            }
        }

        private Vector3 GetBubbleLocation(int x, int y)
        {
            var offset = ((maxY + y) & 1) * BUBBLE_SIZE / 2.0f;
            return new Vector3(
                (BUBBLE_SIZE / 2.0f) + x * BUBBLE_SIZE + offset + 2,
                (y - maxY) * BUBBLE_SIZE * MathUtil.COS_30_DEGREES - (BUBBLE_SIZE / 2.0f) - 2
            );
        }
    }
}
