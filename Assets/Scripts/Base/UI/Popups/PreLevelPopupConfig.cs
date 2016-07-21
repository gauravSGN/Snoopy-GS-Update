namespace UI.Popup
{
    public class PreLevelPopupConfig : PopupConfig
    {
        public int level;
        public long stars;
        public string nextScene;

        override public PopupType type { get { return PopupType.PreLevel; } }
    }
}