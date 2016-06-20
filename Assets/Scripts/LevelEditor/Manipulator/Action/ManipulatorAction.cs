namespace LevelEditor.Manipulator
{
    public interface ManipulatorAction
    {
        void Perform(LevelManipulator manipulator, int x, int y);
    }
}
