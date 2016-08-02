namespace UI.Popup
{
    public class SettingsPopupConfig : PopupConfig
    {
        public string title;
        override public PopupType Type { get { return PopupType.Settings; } }
    }
}