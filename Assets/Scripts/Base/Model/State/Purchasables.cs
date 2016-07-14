using System;
using UnityEngine;
using System.Collections;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Purchasables : PersistableStateHandler
    {
        private static bool replenishing = false;

        public long hearts
        {
            get { return GetValue<long>("hearts", GlobalState.Instance.Config.purchasables.maxHearts); }
            set
            {
                var oldHearts = hearts;
                var maxHearts = GlobalState.Instance.Config.purchasables.maxHearts;
                var newHearts = Math.Min(Math.Max(value, 0), maxHearts);

                if ((newHearts > oldHearts) || (oldHearts == maxHearts))
                {
                    lastTimeHeartAwarded = GetUnixTime();
                }

                SetValue<long>("hearts", newHearts);
                Save();

                GlobalState.Instance.RunCoroutine(ReplenishHeartOverTime());
            }
        }

        public long lastTimeHeartAwarded
        {
            get { return GetValue<long>("lastTimeHeartAwarded", 0); }

            // We avoid using SetValue here so NotifyDispatchers is only called once when hearts are modified
            private set { state["lastTimeHeartAwarded"] = (long)value; }
        }

        public long secondsUntilNextHeart
        {
            get
            {
                long secondsRemaining = -1;

                if (hearts < GlobalState.Instance.Config.purchasables.maxHearts)
                {
                    var nextHeartTime = lastTimeHeartAwarded + GlobalState.Instance.Config.purchasables.secondsPerHeart;
                    secondsRemaining = Math.Max(0, (nextHeartTime - GetUnixTime()));
                }

                return secondsRemaining;
            }
        }

        public Purchasables(Data state, Action<Observable> initialListener = null) : base(state, initialListener)
        {
        }

        public IEnumerator ReplenishHeartOverTime()
        {
            if (!replenishing && (hearts < GlobalState.Instance.Config.purchasables.maxHearts))
            {
                replenishing = true;

                while (secondsUntilNextHeart > 0)
                {
                    yield return new WaitForSeconds(1);
                    NotifyListeners();
                }

                replenishing = false;
                hearts++;
            }
        }

        public void ReplenishHearts()
        {
            var secondsPerHeart = GlobalState.Instance.Config.purchasables.secondsPerHeart;
            long heartsToReplenish = (long)((GetUnixTime() - lastTimeHeartAwarded) / secondsPerHeart);

            heartsToReplenish = Math.Min(Math.Max(heartsToReplenish, 0), GlobalState.Instance.Config.purchasables.maxHearts - hearts);

            if (heartsToReplenish > 0)
            {
                lastTimeHeartAwarded += (heartsToReplenish * secondsPerHeart);
                SetValue<long>("hearts", hearts + heartsToReplenish);
                Save();
            }

            GlobalState.Instance.RunCoroutine(ReplenishHeartOverTime());
        }

        private long GetUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}