namespace UI.Popup
{
    public class SettingsPopupConfig : PopupConfig
    {
        public string userID;
        public string appVersion;

        override public PopupType Type { get { return PopupType.Settings; } }
    }
}