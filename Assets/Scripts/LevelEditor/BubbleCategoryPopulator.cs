using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public sealed class BubbleCategoryPopulator : MonoBehaviour
    {
        [SerializeField]
        private BubbleFactory factory;

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

            categoryList.AddOptions(options);
        }

        private void CreateButtonPanels()
        {
            foreach (var category in EnumExtensions.GetValues<BubbleCategory>())
            {
                CreateButtonPanel(category);
            }
        }

        private void CreateButtonPanel(BubbleCategory category)
        {
            var panel = Instantiate(listPrefab);
            panel.name = category.ToString();
            panel.transform.SetParent(buttonContainer, false);

            var bubbles = factory.Bubbles.Where(b => b.category == category).OrderBy(b => b.type);
            foreach (var definition in bubbles)
            {
                CreateButtonFromDefinition(panel, definition);
            }
        }

        private void CreateButtonFromDefinition(GameObject panel, BubbleDefinition definition)
        {
            var prefabSprite = definition.prefab.GetComponentInChildren<SpriteRenderer>();

            if (prefabSprite != null)
            {
                var button = Instantiate(buttonPrefab);

                button.name = definition.type.ToString();
                button.GetComponent<Image>().sprite = prefabSprite.sprite;
                button.GetComponent<Button>().onClick.AddListener(() => manipulator.SetBubbleType(definition.Type));
                button.transform.SetParent(panel.transform, false);
            }
        }
    }
}
