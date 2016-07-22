using Model;
using Service;
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
            var key = BubbleData.GetKey(x, y);

            manipulator.Models.Remove(key);

            if (manipulator.Views.ContainsKey(key))
            {
                GameObject.Destroy(manipulator.Views[key]);
                manipulator.Views.Remove(key);
            }

            GlobalState.Instance.Services.Get<EventService>().Dispatch(new LevelModifiedEvent());
        }

        public void PerformAlternate(LevelManipulator manipulator, int x, int y)
        {
            Perform(manipulator, x, y);
        }
    }
}
