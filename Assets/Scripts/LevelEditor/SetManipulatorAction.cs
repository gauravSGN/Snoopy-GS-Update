using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LevelEditor.Manipulator;

namespace LevelEditor
{
    public class SetManipulatorAction : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private ManipulatorActionType type;

        private Button button;

        public void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            manipulator.SetActionType(type);
        }
    }
}
