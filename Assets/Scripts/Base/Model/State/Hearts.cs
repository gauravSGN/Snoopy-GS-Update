using Util;
using System;
using UnityEngine;
using System.Collections;
using Data = System.Collections.Generic.IDictionary<string, object>;

namespace State
{
    public class Hearts : StateHandler
    {
        private bool replenishing = false;

        private const string QUANTITY = "quantity";
        private const string LAST_TIME_HEART_AWARDED = "lastTimeHeartAwarded";

        public long quantity
        {
            get { return GetValue<long>(QUANTITY, GlobalState.Instance.Config.purchasables.maxHearts); }
            set
            {
                var oldHearts = quantity;
                var maxHearts = GlobalState.Instance.Config.purchasables.maxHearts;
                var newHearts = Math.Min(Math.Max(value, 0), maxHearts);

                if ((newHearts > oldHearts) || (oldHearts == maxHearts))
                {
                    lastTimeHeartAwarded = DateTimeUtil.GetUnixTime();
                }

                SetValue<long>(QUANTITY, newHearts);

                GlobalState.Instance.RunCoroutine(ReplenishOverTime());
            }
        }

        public long lastTimeHeartAwarded
        {
            get { return GetValue<long>(LAST_TIME_HEART_AWARDED, 0); }

            // We avoid using SetValue here so NotifyDispatchers is only called once when hearts are modified
            private set { state[LAST_TIME_HEART_AWARDED] = (long)value; }
        }

        public long secondsUntilNextHeart
        {
            get
            {
                long secondsRemaining = -1;

                if (quantity < GlobalState.Instance.Config.purchasables.maxHearts)
                {
                    var nextHeartTime = (lastTimeHeartAwarded + GlobalState.Instance.Config.purchasables.secondsPerHeart);
                    secondsRemaining = Math.Max(0, (nextHeartTime - DateTimeUtil.GetUnixTime()));
                }

                return secondsRemaining;
            }
        }

        public Hearts(Data topLevelState, Action<Observable> initialListener = null) : base(topLevelState, initialListener)
        {
        }

        public IEnumerator ReplenishOverTime()
        {
            if (!replenishing && (quantity < GlobalState.Instance.Config.purchasables.maxHearts))
            {
                replenishing = true;

                while (secondsUntilNextHeart > 0)
                {
                    yield return new WaitForSeconds(1);
                    NotifyListeners();
                }

                replenishing = false;
                quantity++;
            }
        }

        public void Replenish()
        {
            var secondsPerHeart = GlobalState.Instance.Config.purchasables.secondsPerHeart;
            long heartsToReplenish = (long)((DateTimeUtil.GetUnixTime() - lastTimeHeartAwarded) / secondsPerHeart);

            heartsToReplenish = Math.Min(Math.Max(heartsToReplenish, 0),
                                         (GlobalState.Instance.Config.purchasables.maxHearts - quantity));

            if (heartsToReplenish > 0)
            {
                lastTimeHeartAwarded += (heartsToReplenish * secondsPerHeart);
                SetValue<long>(QUANTITY, (quantity + heartsToReplenish));
            }

            GlobalState.Instance.RunCoroutine(ReplenishOverTime());
        }
    }
}