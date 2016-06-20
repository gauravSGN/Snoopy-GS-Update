using LevelEditor.Manipulator;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ActionButtonPopulator : MonoBehaviour
    {
        [SerializeField]
        private RectTransform buttonContainer;

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private GameObject buttonPrefab;

        void Start()
        {
            foreach (var enumValue in EnumExtensions.GetValues<ManipulatorActionType>())
            {
                var action = manipulator.ActionFactory.Create(enumValue);

                if (action != null)
                {
                    CreateButtonForType(action, enumValue);
                }
            }
        }

        private void CreateButtonForType(ManipulatorAction action, ManipulatorActionType type)
        {
            var instance = Instantiate(buttonPrefab);

            instance.name = type.ToString();
            instance.transform.SetParent(buttonContainer, false);

            instance.GetComponent<Button>().onClick.AddListener(() => manipulator.SetActionType(type));
            instance.GetComponent<Image>().sprite = action.ButtonSprite;
        }
    }
}
