using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.Manipulator
{
    public class PerformActionComponent : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private ManipulatorActionType actionType;

        public void Start()
        {
            GetComponent<Button>().onClick.AddListener(PerformAction);
        }

        private void PerformAction()
        {
            manipulator.BeginNewAction();
            var action = manipulator.ActionFactory.Create(actionType);

            if (action != null)
            {
                action.Perform(manipulator, 0, 0);
            }
        }
    }
}
