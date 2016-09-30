using Paths;

namespace Snoopy.BossMode
{
    sealed public class SetBossPathEvent : GameEvent
    {
        public NodeTrackPath path;

        public SetBossPathEvent(NodeTrackPath path)
        {
            this.path = path;
        }
    }
}
