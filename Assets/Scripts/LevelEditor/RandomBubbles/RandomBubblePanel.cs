using UnityEngine;
using System.Collections.Generic;
using Model;

namespace LevelEditor
{
    public class RandomBubblePanel : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private GameObject groupPrefab;

        [SerializeField]
        private RectTransform contents;

        [SerializeField]
        private float rowHeight;

        private List<RandomBubbleDefinition> definitions = new List<RandomBubbleDefinition>();
        private List<RandomBubbleGroup> groups = new List<RandomBubbleGroup>();

        public void Start()
        {
            definitions.Add(new RandomBubbleDefinition());
            var instance = Instantiate(groupPrefab);
            instance.transform.SetParent(contents, false);
            var group = instance.GetComponent<RandomBubbleGroup>();
            group.Initialize(manipulator.BubbleFactory, definitions[0]);
            groups.Add(group);

            contents.sizeDelta = new Vector2(0.0f, rowHeight * groups.Count);
        }
    }
}
