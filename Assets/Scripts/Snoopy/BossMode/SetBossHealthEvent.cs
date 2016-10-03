using UnityEngine;
using System.Collections.Generic;

namespace Snoopy.BossMode
{
    sealed public class SetBossHealthEvent : GameEvent
    {
        public int health;

        public SetBossHealthEvent(int health)
        {
            this.health = health;
        }
    }
}
