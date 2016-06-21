using UnityEngine;

public class LevelStateTextUpdater : TextUpdater
{
    [SerializeField]
    private Level level;

    protected override void Start()
    {
        base.Start();

        Target = level.levelState;
    }
}
