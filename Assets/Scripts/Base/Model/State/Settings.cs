namespace State
{
    public class Settings : PersistableStateHandler
    {
        private const string SFX_ON = "sfxOn";
        private const string MUSIC_ON = "musicOn";

        public bool musicOn
        {
            get { return GetValue<bool>(MUSIC_ON, true); }
            set { SetValue<bool>(MUSIC_ON, value); }
        }

        public bool sfxOn
        {
            get { return GetValue<bool>(SFX_ON, true); }
            set { SetValue<bool>(SFX_ON, value); }
        }
    }
}