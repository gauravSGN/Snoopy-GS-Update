using UnityEngine;

public class LevelStateTextUpdater : UITextUpdater
{
    [SerializeField]
    private Level level;

    override protected void Start()
    {
        base.Start();

        Target = level.levelState;
    }
}
