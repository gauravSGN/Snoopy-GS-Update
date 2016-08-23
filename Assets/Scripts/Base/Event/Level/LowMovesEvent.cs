sealed public class LowMovesEvent : GameEvent
{
    public int moves;

    public LowMovesEvent(int moves)
    {
        this.moves = moves;
    }
}
