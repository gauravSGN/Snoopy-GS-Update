using UnityEngine;

namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceBubble)]
    public class PlaceBubbleAction : ManipulatorAction
    {
        public Sprite ButtonSprite
        {
            get { return Resources.Load("Textures/UI/PlaceBubbleButton", typeof(Sprite)) as Sprite; }
        }

        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            manipulator.RemoveBubble(x, y);
            manipulator.PlaceBubble(x, y, manipulator.BubbleType);
        }
    }
}
