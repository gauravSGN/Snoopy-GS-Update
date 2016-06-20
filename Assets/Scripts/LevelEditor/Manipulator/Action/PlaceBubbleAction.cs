namespace LevelEditor.Manipulator
{
    [ManipulatorAction(ManipulatorActionType.PlaceBubble)]
    public class PlaceBubbleAction : ManipulatorAction
    {
        public void Perform(LevelManipulator manipulator, int x, int y)
        {
            manipulator.RemoveBubble(x, y);
            manipulator.PlaceBubble(x, y, manipulator.BubbleType);
        }
    }
}
