using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BubbleContent;
using Model;

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

            SetActiveCategory(BubbleCategory.Default);
        }

        private void SetupCategoryList()
        {
            var options = new List<Dropdown.OptionData>();

            categoryList.ClearOptions();

            foreach (var category in EnumExtensions.GetValues<BubbleCategory>())
            {
                options.Add(new Dropdown.OptionData(category.ToString()));
            }

            options.Add(new Dropdown.OptionData("Bubble Contents"));

            categoryList.AddOptions(options);
        }

        private void CreateButtonPanels()
        {
            foreach (var category in EnumExtensions.GetValues<BubbleCategory>())
            {
                CreateButtonPanel<BubbleDefinition, BubbleType>(
                    category.ToString(),
                    manipulator.BubbleFactory.Bubbles.Where(b => b.category == category),
                    SetBubbleType
                );
            }

            CreateButtonPanel<BubbleContentDefinition, BubbleContentType>(
                "Bubble Contents",
                manipulator.ContentFactory.Contents,
                SetContentType
            );
        }

        private void CreateButtonPanel<T, U>(string name, IEnumerable<T> items, Action<U> action)
            where T : GameObjectDefinition<U>
        {
            var panel = Instantiate(listPrefab);
            panel.name = name;
            panel.transform.SetParent(buttonContainer, false);

            foreach (var definition in items.OrderBy(i => i.Type))
            {
                CreateButtonFromDefinition(panel, definition, action);
            }
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
        }

        private void SetContentType(BubbleContentType type)
        {
            manipulator.SetContentType(type);
        }
    }
}
