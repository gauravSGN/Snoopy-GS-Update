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
            var key = y << 4 | x;

            manipulator.Models.Remove(key);

            if (manipulator.Views.ContainsKey(key))
            {
                GameObject.Destroy(manipulator.Views[key]);
                manipulator.Views.Remove(key);
            }
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            Perform(manipulator, x, y);
        }
    }
}
