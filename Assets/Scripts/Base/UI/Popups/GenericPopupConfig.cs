using System;
using System.Collections.Generic;

namespace UI.Popup
{
    public class GenericPopupConfig : PopupConfig
    {
        public string title;
        public string mainText;
        public List<Action> closeActions;
        public List<Action> affirmativeActions;
    }
}