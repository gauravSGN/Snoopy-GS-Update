using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using LevelEditor.Manipulator;
using Model;
using Service;

namespace LevelEditor.Menu
{
    sealed public class RandomBubbleWidget : MenuWidgetBase, MenuWidget
    {
        private const string PREFAB_PATH = "LevelEditor/Menu/RandomContextPanel";

        public int SortOrder { get { return 99; } }

        private GameObject prefab;
        private BubbleData data;
        private RandomBubbleDefinition definition;
        private PlaceBubbleAction placer = new PlaceBubbleAction();

        public bool IsValidFor(BubbleData bubble)
        {
            return (bubble.modifiers != null) && bubble.modifiers.Any(m => m.type == BubbleModifierType.Random);
        }

        public GameObject CreateWidget(BubbleData bubble)
        {
            prefab = prefab ?? GlobalState.Instance.Services.Get<AssetService>().LoadAsset<GameObject>(PREFAB_PATH);
            definition = CreateDefinition(bubble);
            data = bubble;

            var colors = GetColors();
            var panel = GameObject.Instantiate(prefab);
            var weightFields = CreateFields(panel, colors);

            InitializeButtons(panel);
            InitializeFields(colors, weightFields);
            InitializeExclusions(panel);

            return panel;
        }

        private Color[] GetColors()
        {
            return Manipulator.BubbleFactory.Bubbles
                .Where(b => b.category == BubbleCategory.Basic)
                .Where(d => d.Prefab.GetComponentInChildren<SpriteRenderer>() != null)
                .Select(d => d.BaseColor)
                .ToArray();
        }

        private RandomBubbleDefinition CreateDefinition(BubbleData bubble)
        {
            var modifier = bubble.modifiers.First(m => m.type == BubbleModifierType.Random);
            var definition = Manipulator.Randoms[int.Parse(modifier.data)];

            return definition.Clone();
        }

        private List<GameObject> CreateFields(GameObject panel, Color[] colors)
        {
            var weightFields = new List<GameObject> { panel.transform.FindChild("WeightField").gameObject };
            var rect = weightFields[0].GetComponent<RectTransform>().rect;
            var halfCount = (colors.Length + 1) / 2;

            while (weightFields.Count < colors.Length)
            {
                var field = GameObject.Instantiate(weightFields[0]);
                field.transform.SetParent(panel.transform, false);

                field.transform.localPosition = new Vector3(
                    (weightFields.Count / halfCount) * rect.width,
                    rect.y - 2.0f - (weightFields.Count % halfCount) * rect.height
                );

                weightFields.Add(field);
            }

            var rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, -rect.y + 6.0f + (halfCount + 1) * rect.height);

            return weightFields;
        }

        private void InitializeButtons(GameObject panel)
        {
            var onceEachButton = panel.transform.FindChild("OnceEachButton").GetComponent<Button>();
            var saveButton = panel.transform.FindChild("SaveButton").GetComponent<Button>();

            SetOnceEachText(onceEachButton);

            onceEachButton.onClick.AddListener(() =>
            {
                    OnOnceEachClick();
                    SetOnceEachText(onceEachButton);
            });

            saveButton.onClick.AddListener(OnSaveClick);
        }

        private void SetOnceEachText(Button button)
        {
            button.GetComponentInChildren<Text>().text = definition.rollType.ToString();
        }

        private void InitializeFields(Color[] colors, List<GameObject> fields)
        {
            var count = colors.Length;

            for (var index = 0; index < count; index++)
            {
                var field = fields[index];

                var color = colors[index];
                field.transform.FindChild("Dot").GetComponent<Image>().color = color;
                field.GetComponentInChildren<BubbleWeightElement>().Initialize(definition.weights, index, color);
            }
        }

        private void InitializeExclusions(GameObject panel)
        {
            var exclusions = panel.transform.FindChild("ExclusionsPanel").GetComponent<RandomExclusionPanel>();
            var halfCount = (definition.weights.Length + 1) / 2;

            exclusions.transform.localPosition = new Vector3(2.0f, -28.0f - (halfCount * 24.0f));
            exclusions.Initialize(definition.exclusions);
        }

        private void OnOnceEachClick()
        {
            definition.rollType = (ChainedRandomizer<BubbleType>.SelectionMethod)(1 - (int)definition.rollType);
        }

        private void OnSaveClick()
        {
            var match = Manipulator.Randoms.FirstOrDefault(r => r.Compare(definition));

            if (match == null)
            {
                match = definition;
                Manipulator.Randoms.Add(definition);
            }

            GlobalState.Instance.Services.Get<Service.EventService>().Dispatch(new RandomBubblesChangedEvent());

            var modifier = data.modifiers.First(m => m.type == BubbleModifierType.Random);
            modifier.data = Manipulator.Randoms.IndexOf(match).ToString();

            PerformNonvolatileAction(() => {
                Manipulator.SetBubbleType(data.Type);
                placer.Perform(Manipulator, data.X, data.Y);
            });

            Complete();
        }
    }
}
