public class LevelStateTextUpdater : TextUpdater
{
    public Level level;

    override protected void Start()
    {
        base.Start();

        Target = level.levelState;
    }
}
