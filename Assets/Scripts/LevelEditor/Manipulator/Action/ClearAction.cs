using UnityEngine;

namespace LevelEditor.Manipulator
{
    [HideAction]
    [ManipulatorAction(ManipulatorActionType.Clear)]
    public class ClearAction : ManipulatorAction
    {
        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            foreach (var pair in manipulator.Views)
            {
                GameObject.Destroy(pair.Value);
            }

            manipulator.Models.Clear();
            manipulator.Views.Clear();

            GlobalState.EventService.Dispatch(new LevelModifiedEvent());
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            Perform(manipulator, x, y);
        }
    }
}
