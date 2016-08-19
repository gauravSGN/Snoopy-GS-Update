using UnityEngine;
using System.Collections.Generic;

namespace Registry
{
    static public class BlockadeEvent
    {
        sealed public class PopupsBlocked : GameEvent {}
        sealed public class PopupsUnblocked : GameEvent {}
        sealed public class SceneChangeBlocked : GameEvent {}
        sealed public class SceneChangeUnblocked : GameEvent {}
    }
}
