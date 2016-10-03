using Spine;
using Model;
using Actions;
using Spine.Unity;
using UnityEngine;
using System.Collections.Generic;

namespace Snoopy.BossMode
{
    sealed public class BossController : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private int health;

        [SerializeField]
        private string[] damageSkins;

        private Skeleton skeleton;
        private BossTrackFollowAction follower = new BossTrackFollowAction();

        public void Start()
        {
            skeleton = GetComponentInChildren<SkeletonAnimator>().skeleton;

            GlobalState.EventService.AddEventHandler<SetBossHealthEvent>(OnSetBossHealth);
            GlobalState.EventService.AddEventHandler<SetBossPathEvent>(OnSetBossPath);

            SetCurrentSkin();
        }

        private void SetCurrentSkin()
        {
            var skinIndex = Mathf.RoundToInt((1.0f - (float)health / (float)maxHealth) * (damageSkins.Length - 1.0f));
            skeleton.SetSkin(damageSkins[skinIndex]);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var bubbleSnap = collision.collider.GetComponent<BubbleSnap>();

            if (bubbleSnap != null)
            {
                follower.Stop();
                transform.rotation = Quaternion.identity;
                health -= 1;

                GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
                BubbleReactionEvent.Dispatch(Reaction.ReactionPriority.Cull,
                                             bubbleSnap.GetComponent<BubbleModelBehaviour>().Model);

                bubbleSnap.CompleteSnap();
                SetCurrentSkin();

                GlobalState.EventService.Dispatch(new DamageBossEvent());
            }
        }

        private void OnSetBossHealth(SetBossHealthEvent gameEvent)
        {
            maxHealth = gameEvent.health;
            health = Mathf.Min(health, maxHealth);
        }

        private void OnSetBossPath(SetBossPathEvent gameEvent)
        {
            follower.Path = gameEvent.path;
            follower.Start();

            GetComponent<ActionQueue>().AddGameAction(follower);
        }
    }
}
