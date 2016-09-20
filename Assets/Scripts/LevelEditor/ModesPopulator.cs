using Model;
using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;
using System.Collections.Generic;

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
            button.transform.SetParent(transform, false);

            var toggle = button.GetComponent<Toggle>();

            toggle.group = GetComponent<ToggleGroup>();
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    manipulator.SetModifier(modifier);
                    manipulator.SetActionType(ManipulatorActionType.PlaceModifier);
                }
            });
        }
    }
}
