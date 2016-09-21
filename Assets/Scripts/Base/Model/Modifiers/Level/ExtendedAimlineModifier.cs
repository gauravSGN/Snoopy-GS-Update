using Model;
using UnityEngine;

namespace Modifiers
{
    [LevelModifierAttribute(LevelModifierType.ExtendedAimline)]
    sealed public class ExtendedAimlineModifier : LevelModifier
    {
        private AimLine aimline;

        public ExtendedAimlineModifier()
        {
            aimline = GameObject.FindObjectOfType<AimLine>();
            OnReadyForNextBubble();

            GlobalState.EventService.AddEventHandler<ReadyForNextBubbleEvent>(OnReadyForNextBubble);
        }

        public void SetData(string data)
        {
            // This modifier doesn't take any additional data
        }

        private void OnReadyForNextBubble()
        {
            aimline.ModifyAimline(GlobalState.Instance.Config.aimline.extended);
        }
    }
}
