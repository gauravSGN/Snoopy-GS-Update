using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BubbleContent;
using Model;
using LevelEditor.Manipulator;

namespace LevelEditor
{
    sealed public class BubbleCategoryPopulator : MonoBehaviour
    {
        [SerializeField]
        private Dropdown categoryList;

        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject listPrefab;

        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private LevelManipulator manipulator;

        public void SetActiveCategory(BubbleCategory category)
        {
            SetActiveCategory(category.ToString());
        }

        public void SetActiveCategory(string name)
        {
            foreach (Transform child in buttonContainer.transform)
            {
                child.gameObject.SetActive(child.name == name);
            }
        }

        public void OnCategoryChanged()
        {
            SetActiveCategory(categoryList.options[categoryList.value].text);
        }

        private void Start()
        {
            SetupCategoryList();
            CreateButtonPanels();

            SetActiveCategory(BubbleCategory.Goal);
        }

        private void SetupCategoryList()
        {
            var options = new List<Dropdown.OptionData>();

            categoryList.ClearOptions();

            foreach (var category in EnumExtensions.GetValues<BubbleCategory>().Where(c => c != BubbleCategory.Basic))
            {
                options.Add(new Dropdown.OptionData(category.ToString()));
            }

            categoryList.AddOptions(options);
        }

        private void CreateButtonPanels()
        {
            var bubbleDefs = manipulator.BubbleFactory.Bubbles;

            foreach (var category in EnumExtensions.GetValues<BubbleCategory>().Where(c => c != BubbleCategory.Basic))
            {
                CreateButtonPanel<BubbleDefinition, BubbleType>(
                    category.ToString(),
                    bubbleDefs.Where(b => (b.category == BubbleCategory.Basic) || (b.category == category)),
                    SetBubbleType
                );
            }

            CreateButtonPanel<BubbleContentDefinition, BubbleContentType>(
                BubbleCategory.Goal.ToString(),
                manipulator.ContentFactory.Contents,
                SetContentType
            );
        }

        private void CreateButtonPanel<T, U>(string name, IEnumerable<T> items, Action<U> action)
            where T : GameObjectDefinition<U>
        {
            var panel = GetOrCreatePanel(name);

            foreach (var definition in items.OrderBy(i => i.Type))
            {
                CreateButtonFromDefinition(panel, definition, action);
            }
        }

        private GameObject GetOrCreatePanel(string name)
        {
            foreach (Transform child in buttonContainer.transform)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }
            }

            var panel = Instantiate(listPrefab);
            panel.transform.SetParent(buttonContainer, false);
            panel.name = name;

            return panel;
        }

        private void CreateButtonFromDefinition<T>(GameObject panel, GameObjectDefinition<T> definition, Action<T> action)
        {
            var prefabSprite = definition.Prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabSprite != null)
            {
                var button = Instantiate(buttonPrefab);

                button.name = definition.Type.ToString();
                button.GetComponent<Image>().sprite = prefabSprite.sprite;
                button.GetComponent<Button>().onClick.AddListener(() => action(definition.Type));
                button.transform.SetParent(panel.transform, false);
            }
        }

        private void SetBubbleType(BubbleType type)
        {
            manipulator.SetBubbleType(type);
            manipulator.SetActionType(ManipulatorActionType.PlaceBubble);
        }

        private void SetContentType(BubbleContentType type)
        {
            manipulator.SetContentType(type);
            manipulator.SetActionType(ManipulatorActionType.PlaceBubble);
        }
    }
}
