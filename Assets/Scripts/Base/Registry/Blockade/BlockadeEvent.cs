namespace Registry
{
    static public class BlockadeEvent
    {
        sealed public class PopupsBlocked : GameEvent {}
        sealed public class PopupsUnblocked : GameEvent {}
        sealed public class SceneChangeBlocked : GameEvent {}
        sealed public class SceneChangeUnblocked : GameEvent {}
        sealed public class InputBlocked : GameEvent {}
        sealed public class InputUnblocked : GameEvent {}
        sealed public class ReactionsBlocked : GameEvent {}
        sealed public class ReactionsUnblocked: GameEvent {}
    }
}
