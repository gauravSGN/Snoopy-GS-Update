using UnityEngine;

public class LevelStateTextUpdater : TextUpdater
{
    [SerializeField]
    private Level level;

    override protected void Start()
    {
        base.Start();

        Target = level.levelState;
    }
}
