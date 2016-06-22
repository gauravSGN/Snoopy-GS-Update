using System;

namespace LevelEditor.Manipulator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    sealed public class ManipulatorActionAttribute : Attribute
    {
        public ManipulatorActionType ActionType { get; private set; }

        public ManipulatorActionAttribute(ManipulatorActionType actionType)
        {
            ActionType = actionType;
        }
    }
}
