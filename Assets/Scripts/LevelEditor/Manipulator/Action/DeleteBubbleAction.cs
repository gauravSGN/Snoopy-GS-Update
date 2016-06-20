using UnityEngine;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.DeleteBubble)]
    public class DeleteBubbleAction : ManipulatorAction
    {
        public Sprite ButtonSprite
        {
            get { return Resources.Load("Textures/UI/DeleteBubbleButton", typeof(Sprite)) as Sprite; }
        }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            manipulator.RemoveBubble(x, y);
        }
    }
}
