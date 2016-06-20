namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.DeleteBubble)]
    public class DeleteBubbleAction : ManipulatorAction
    {
        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            manipulator.RemoveBubble(x, y);
        }
    }
}
