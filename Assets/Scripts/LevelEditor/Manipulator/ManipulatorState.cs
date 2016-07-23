using UnityEngine;
using System.Collections.Generic;
using Model;
using LevelEditor.Manipulator;

namespace LevelEditor
{
    sealed public class ManipulatorState
    {
        public BubbleType BubbleType { get; set; }
        public BubbleModifierInfo Modifier { get; set; }
        public ManipulatorActionType ActionType { get; set; }
        public ManipulatorAction Action { get; set; }

        public ManipulatorState Clone()
        {
            return new ManipulatorState
            {
                BubbleType = BubbleType,
                Modifier = Modifier,
                ActionType = ActionType,
                Action = Action,
            };
        }
    }
}
