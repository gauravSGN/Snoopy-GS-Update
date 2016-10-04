using UnityEngine;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class SpawnPuzzle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var placer = animator.GetComponent<BossBubblePlacer>();
            System.Action onComplete;

            onComplete = delegate
            {
                placer.OnComplete -= onComplete;
                animator.SetTrigger("PuzzleSpawned");
            };

            placer.OnComplete += onComplete;
            placer.SpawnPuzzle();
        }
    }
}
