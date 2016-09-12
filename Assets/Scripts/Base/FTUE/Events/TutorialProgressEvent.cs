namespace FTUE
{
    sealed public class TutorialProgressEvent : GameEvent
    {
        public TutorialTrigger trigger;
        public int level;
        public string identifier;

        public TutorialProgressEvent(TutorialTrigger trigger)
        {
            this.trigger = trigger;
        }

        public TutorialProgressEvent(TutorialTrigger trigger, int level) : this(trigger)
        {
            this.level = level;
        }

        public TutorialProgressEvent(TutorialTrigger trigger, string identifier) : this(trigger)
        {
            this.identifier = identifier;
        }
    }
}
