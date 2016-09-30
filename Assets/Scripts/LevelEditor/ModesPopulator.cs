using Model;
using Modifiers;
using UnityEngine;
using UnityEngine.UI;
using LevelEditor.Manipulator;

namespace LevelEditor
{
    sealed public class ModesPopulator : MonoBehaviour
    {
        [SerializeField]
        private GameObject bubbleButtonPrefab;

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private BubbleModifierList modifierList;

        public void Start()
        {
            foreach (var modifier in modifierList.Items)
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
