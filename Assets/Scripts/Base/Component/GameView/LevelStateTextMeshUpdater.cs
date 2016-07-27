using UnityEngine;

public class LevelStateTextMeshUpdater : TextMeshUpdater
{
    [SerializeField]
    private Level level;

    override protected void Start()
    {
        base.Start();
        Target = level.levelState;
    }
}
