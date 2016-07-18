using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Model;

namespace LevelEditor
{
    sealed public class ModesPopulator : MonoBehaviour
    {
        [SerializeField]
        private GameObject bubbleButtonPrefab;

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private List<BubbleModifierDefinition> modifiers;

        public void Start()
        {
            foreach (var modifier in modifiers)
            {
                CreateModifierButton(modifier);
            }
        }

        private void CreateModifierButton(BubbleModifierDefinition modifier)
        {
            var button = Instantiate(bubbleButtonPrefab);

            button.name = modifier.Type.ToString();
            button.GetComponent<Image>().sprite = modifier.Sprite;
            var toggle = button.GetComponent<Toggle>();
            toggle.group = GetComponent<ToggleGroup>();
            //toggle.onValueChanged.AddListener((v) => { if (v) action(definition.Type); });
            button.transform.SetParent(transform, false);
        }
    }
}
