using UnityEngine;

namespace LevelEditor.Manipulator
{
    [HideAction]
    [ManipulatorAction(ManipulatorActionType.Clear)]
    public class ClearAction : ManipulatorAction
    {
        public Sprite ButtonSprite { get { return null; } }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            foreach (var pair in manipulator.Views)
            {
                GameObject.Destroy(pair.Value);
            }

            manipulator.Models.Clear();
            manipulator.Views.Clear();
        }
    }
}
