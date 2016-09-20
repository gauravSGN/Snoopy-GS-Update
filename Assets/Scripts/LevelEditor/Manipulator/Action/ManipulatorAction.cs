namespace LevelEditor.Manipulator
{
    public interface ManipulatorAction
    {
        void Perform(LevelManipulator manipulator, int x, int y);
        void PerformAlternate(LevelManipulator manipulator, int x, int y);
    }
}
