﻿using UnityEngine;
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

        [SerializeField]
        private RandomExclusionPanel exclusionPanel;

        private RandomBubbleDefinition definition;

        public string Label
        {
            get { return groupLabel.text; }
            set { groupLabel.text = value; }
        }

        public int Count
        {
            get { return int.Parse(countLabel.text); }
            set { countLabel.text = value.ToString(); }
        }

        public Button DeleteButton { get { return deleteButton; } }

        public void Start()
        {
            onceEachButton.onClick.AddListener(OnOnceEachButtonClick);
            SetOnceEachText();
        }

        public void Initialize(BubbleFactory factory, RandomBubbleDefinition definition)
        {
            this.definition = definition;

            CreateWeightElements(factory, definition.weights);
            exclusionPanel.Initialize(definition.exclusions);
        }

        private void OnOnceEachButtonClick()
        {
            definition.rollType = (RandomBubbleDefinition.RollType)(1 - (int)definition.rollType);
            SetOnceEachText();
        }

        private void SetOnceEachText()
        {
            onceEachButton.GetComponentInChildren<Text>().text = definition.rollType.ToString().Substring(0, 1);
        }
    }
}