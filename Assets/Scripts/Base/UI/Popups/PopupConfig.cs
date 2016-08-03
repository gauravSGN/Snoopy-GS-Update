using System;
using System.Collections.Generic;

namespace UI.Popup
{
    public class PopupConfig
    {
        public string title;
        public List<Action> closeActions;
        public List<Action> affirmativeActions;

        virtual public PopupPriority Priority { get; set; }
        virtual public PopupType Type { get { return PopupType.Generic; } }
        virtual public bool IgnoreQueue { get { return false; } }
    }
}