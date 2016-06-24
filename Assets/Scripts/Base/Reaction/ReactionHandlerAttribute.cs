using System;

namespace Reaction
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    sealed public class ReactionHandlerAttribute : Attribute
    {
        public ReactionPriority Priority { get; private set; }

        public ReactionHandlerAttribute(ReactionPriority priority)
        {
            Priority = priority;
        }
    }
}
