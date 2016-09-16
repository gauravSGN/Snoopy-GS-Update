namespace Reaction
{
    // Each handler has a reaction and a post reaction that run at each priority
    public enum ReactionPriority
    {
        PreReactionAnimation = 5,

        PowerUp = 8,

        Pop = 10,
        GenericPop = 15,
        ChainPop = 20,

        PhysicsDestroy = 25,

        CullRainbow = 99995,
        Cull = 99999,
    }
}
