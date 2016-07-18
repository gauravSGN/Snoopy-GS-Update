using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace LevelEditor
{
    public abstract class BubbleWeightEditor : MonoBehaviour
    {
        [SerializeField]
        private RectTransform elementContainer;

        [SerializeField]
        private GameObject elementPrefab;

        protected List<BubbleWeightElement> elements = new List<BubbleWeightElement>();

        protected void CreateWeightElements(BubbleFactory factory, int[] values)
        {
            int index = 0;

            foreach (var def in factory.Bubbles.Where(b => b.category == BubbleCategory.Basic))
            {
                var sprite = def.Prefab.GetComponentInChildren<SpriteRenderer>();

                if (sprite != null)
                {
                    var instance = Instantiate(elementPrefab);
                    var element = instance.GetComponent<BubbleWeightElement>();

                    instance.transform.SetParent(elementContainer, false);
                    element.Initialize(values, index, def.BaseColor);

                    elements.Add(element);
                    index++;
                }
            }
        }
    }
}
