namespace FTUE
{
    sealed public class ShowTutorialEvent : GameEvent
    {
        public string id;

        public ShowTutorialEvent(string id)
        {
            this.id = id;
        }
    }
}
