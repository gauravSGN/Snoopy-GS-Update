using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Model;

namespace LevelEditor
{
    public class RandomBubbleGroup : BubbleWeightEditor
    {
        [SerializeField]
        private Text groupLabel;

        [SerializeField]
        private Button onceEachButton;

        [SerializeField]
        private Text countLabel;

        [SerializeField]
        private Button deleteButton;

        private RandomBubbleDefinition definition;

        public void Start()
        {
        }

        public void Initialize(BubbleFactory factory, RandomBubbleDefinition definition)
        {
            this.definition = definition;

            CreateWeightElements(factory, definition.weights);
        }
    }
}
