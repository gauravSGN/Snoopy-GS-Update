namespace Reaction
{
    // The priority immediately after any given bubble reaction is reserved for blocking sequences.
    // The blocking sequence priority needs to be defined to actually run.
    public enum ReactionPriority
    {
        PreReactionAnimation = 5,

        PowerUp = 8,
        PostPowerUpAnimation = 9,

        Pop = 10,
        PostPopAnimation = 11,

        GenericPop = 15,
        PostGenericPopAnimation = 16,

        ChainPop = 20,
        PostChainPopAnimation = 21,

        PhysicsDestroy = 25,

        CullRainbow = 99995,
        Cull = 99999,
    }
}
