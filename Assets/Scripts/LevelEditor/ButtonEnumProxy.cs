using LevelEditor.Manipulator;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ButtonEnumProxy : MonoBehaviour
    {
        [SerializeField]
        private ManipulatorActionType enumValue;

        [SerializeField]
        private LevelManipulator manipulator;

        protected void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => manipulator.SetActionType(enumValue));
        }
    }
}
