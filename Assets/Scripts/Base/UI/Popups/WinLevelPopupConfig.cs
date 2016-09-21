namespace UI.Popup
{
    public class WinLevelPopupConfig : PopupConfig
    {
        public long stars;
        public long score;
        public long topScore;
        public int level;

        override public PopupType Type { get { return PopupType.WinLevel; } }
    }
}