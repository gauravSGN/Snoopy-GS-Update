using Model;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Modifiers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LevelModifierAttribute : Attribute
    {
        public LevelModifierType ModifierType { get; private set; }

        public LevelModifierAttribute(LevelModifierType type)
        {
            ModifierType = type;
        }
    }
}
