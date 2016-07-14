using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace LevelEditor
{
    public class BubbleDotGenerator : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private RectTransform dotsPanel;

        [SerializeField]
        private GameObject dotPrefab;

        public void Start()
        {
            CreateDots();
        }

        private void CreateDots()
        {
            foreach (var def in manipulator.BubbleFactory.Bubbles.Where(b => b.category == BubbleCategory.Basic))
            {
                var sprite = def.Prefab.GetComponentInChildren<SpriteRenderer>();

                if (sprite != null)
                {
                    var instance = Instantiate(dotPrefab);
                    var image = instance.GetComponent<Image>();

                    image.color = def.BaseColor;
                    image.transform.SetParent(dotsPanel, false);
                }
            }
        }
    }
}
