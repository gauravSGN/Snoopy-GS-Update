namespace UI.Popup
{
    public class WinLevelPopupConfig : PopupConfig
    {
        public long stars;
        public long score;
        public long topScore;

        override public PopupType Type { get { return PopupType.WinLevel; } }
    }
}