namespace FTUE.Rewards
{
    sealed public class NullReward : TutorialReward
    {
        public void Apply(int count)
        {
            // Do nothing; this is the default if an invalid reward is specified
        }
    }
}
