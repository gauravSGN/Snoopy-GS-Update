namespace State
{
    sealed public class Tutorials : PersistableStateHandler
    {
        public bool HasCompleted(string tutorialName)
        {
            return GetValue<bool>(tutorialName, false);
        }

        public void MarkCompleted(string tutorialName)
        {
            SetValue<bool>(tutorialName, true);
        }
    }
}
