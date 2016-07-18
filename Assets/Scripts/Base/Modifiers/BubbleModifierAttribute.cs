using System;
using Model;

namespace Modifiers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BubbleModifierAttribute : Attribute
    {
        public BubbleModifierType ModifierType { get; private set; }

        public BubbleModifierAttribute(BubbleModifierType type)
        {
            ModifierType = type;
        }
    }
}
