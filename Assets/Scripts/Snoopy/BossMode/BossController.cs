using Spine;
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

        private Animator animator;
        private Skeleton skeleton;

        public void Start()
        {
            animator = GetComponentInChildren<Animator>();
            skeleton = GetComponentInChildren<SkeletonAnimator>().skeleton;

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
                health -= 1;

                BubbleReactionEvent.Dispatch(Reaction.ReactionPriority.Cull, bubbleSnap.GetComponent<BubbleModelBehaviour>().Model);

                bubbleSnap.CompleteSnap();
                SetCurrentSkin();
            }
        }
    }
}
