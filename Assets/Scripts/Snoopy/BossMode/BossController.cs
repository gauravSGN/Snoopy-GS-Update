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
        public Paths.NodeTrackPath Path { get; private set; }

        [SerializeField]
        private string[] damageSkins;

        private Animator animator;
        private Skeleton skeleton;
        private int maxHealth;

        public void Start()
        {
            animator = GetComponent<Animator>();
            skeleton = GetComponentInChildren<SkeletonAnimator>().skeleton;

            GlobalState.EventService.AddEventHandler<SetBossHealthEvent>(OnSetBossHealth);
            GlobalState.EventService.AddEventHandler<SetBossPathEvent>(OnSetBossPath);
        }

        private void SetCurrentSkin()
        {
            var health = animator.GetInteger("Health");
            var skinIndex = Mathf.RoundToInt((1.0f - (float)health / (float)maxHealth) * (damageSkins.Length - 1.0f));
            skeleton.SetSkin(damageSkins[skinIndex]);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var bubbleSnap = collision.collider.GetComponent<BubbleSnap>();

            if (bubbleSnap != null)
            {
                animator.SetTrigger("Hit");

                GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
                BubbleReactionEvent.Dispatch(Reaction.ReactionPriority.Cull,
                                             bubbleSnap.GetComponent<BubbleModelBehaviour>().Model);

                bubbleSnap.CompleteSnap();
                SetCurrentSkin();
            }
        }

        private void OnSetBossHealth(SetBossHealthEvent gameEvent)
        {
            maxHealth = gameEvent.health;
            animator.SetInteger("Health", maxHealth);
        }

        private void OnSetBossPath(SetBossPathEvent gameEvent)
        {
            Path = gameEvent.path;
        }
    }
}
