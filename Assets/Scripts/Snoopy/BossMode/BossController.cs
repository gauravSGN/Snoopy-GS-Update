using UnityEngine;

namespace Snoopy.BossMode
{
    sealed public class BossController : MonoBehaviour
    {
        public Paths.NodeTrackPath Path { get; private set; }

        private Animator animator;

        public void Start()
        {
            animator = GetComponent<Animator>();

            GlobalState.EventService.AddEventHandler<LevelLoadedEvent>(OnLevelLoaded);
            GlobalState.EventService.AddEventHandler<SetBossHealthEvent>(OnSetBossHealth);
            GlobalState.EventService.AddEventHandler<SetBossPathEvent>(OnSetBossPath);
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
            }
        }

        private void OnLevelLoaded()
        {
            animator.SetTrigger("LevelLoaded");
        }

        private void OnSetBossHealth(SetBossHealthEvent gameEvent)
        {
            animator.SetInteger("MaxHealth", gameEvent.health);
            animator.SetInteger("Health", gameEvent.health);
        }

        private void OnSetBossPath(SetBossPathEvent gameEvent)
        {
            Path = gameEvent.path;
        }
    }
}
