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

        public void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            manipulator.SetActionType(type);
        }
    }
}
