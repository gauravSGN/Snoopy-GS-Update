namespace Event
{
    public class BubbleScoreEvent : GameEvent
    {
        public Bubble bubble;
        public int score;

        public BubbleScoreEvent(Bubble bubble, int score)
        {
            this.bubble = bubble;
            this.score = score;
        }
    }
}
