using Model;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;
using System.Collections.Generic;

namespace LevelEditor
{
    [RequireComponent(typeof(TabSwitcher))]
    sealed public class BubbleCategoryPopulator : MonoBehaviour
    {
        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private Transform categoryContainer;

        [SerializeField]
        private GameObject listPrefab;

        [SerializeField]
        private GameObject bubbleButtonPrefab;

        [SerializeField]
        private GameObject categoryButtonPrefab;

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private GameObject tuningPanel;

        [SerializeField]
        private GameObject modesPanel;

        [SerializeField]
        private GameObject bossModePanel;

        private TabSwitcher tabSwitcher;

        private void Start()
        {
            tabSwitcher = GetComponent<TabSwitcher>();

            CreateToggleButton("Boss Mode", bossModePanel);

            SetupCategoryList();
            CreateButtonPanels();

            CreateToggleButton("Tune", tuningPanel);

            tabSwitcher.SwitchTab(modesPanel);
        }

        private void SetupCategoryList()
        {
            foreach (var category in EnumExtensions.GetValues<BubbleCategory>().Where(c => c != BubbleCategory.Basic))
            {
                CreateCategoryButton(category);
            }
        }

        private void CreateCategoryButton(BubbleCategory category)
        {
            CreateToggleButton(category.ToString(), GetOrCreatePanel(category.ToString()));
        }

        private void CreateToggleButton(string name, GameObject panel)
        {
            var button = Instantiate(categoryButtonPrefab);

            button.transform.SetParent(categoryContainer.transform, false);
            button.name = name;

            button.GetComponent<Button>().onClick.AddListener(() => tabSwitcher.SwitchTab(panel));
            button.GetComponentInChildren<Text>().text = name;
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

        private void CreateButtonFromDefinition<T>(GameObject panel,
                                                   GameObjectDefinition<T> definition,
                                                   Action<T> action)
        {
            var prefabSprite = definition.Prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabSprite != null)
            {
                var button = Instantiate(bubbleButtonPrefab);

                button.name = definition.Type.ToString();
                button.GetComponent<Image>().sprite = prefabSprite.sprite;
                var toggle = button.GetComponent<Toggle>();
                toggle.group = panel.GetComponent<ToggleGroup>();
                toggle.onValueChanged.AddListener((v) => { if (v) { action(definition.Type); } });
                button.transform.SetParent(panel.transform, false);
            }
        }

        private void SetBubbleType(BubbleType type)
        {
            manipulator.SetBubbleType(type);
            manipulator.SetActionType(ManipulatorActionType.PlaceBubble);
        }
    }
}
