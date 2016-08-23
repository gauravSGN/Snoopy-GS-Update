using UnityEngine;
using System.Collections.Generic;

namespace UI.Popup
{
    public class StandalonePopupConfig : PopupConfig
    {
        private PopupType type;

        override public PopupType Type { get { return type; } }

        public StandalonePopupConfig(PopupType type)
        {
            this.type = type;
        }
    }
}
