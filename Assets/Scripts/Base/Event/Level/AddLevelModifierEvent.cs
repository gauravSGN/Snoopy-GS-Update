using Model;
using UnityEngine;
using System.Collections.Generic;

sealed public class AddLevelModifierEvent : GameEvent
{
    public LevelModifierType type;
    public string data;

    public AddLevelModifierEvent(LevelModifierType type, string data)
    {
        this.type = type;
        this.data = data;
    }
}
