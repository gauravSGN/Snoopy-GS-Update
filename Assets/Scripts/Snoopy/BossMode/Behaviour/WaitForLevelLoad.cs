using UnityEngine;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class WaitForLevelLoad : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            System.Action onLevelLoaded;

            onLevelLoaded = delegate
            {
                GlobalState.EventService.RemoveEventHandler<LevelLoadedEvent>(onLevelLoaded);

                Util.FrameUtil.OnNextFrame(() => { animator.SetTrigger("LevelLoaded"); });
            };

            GlobalState.EventService.AddEventHandler<LevelLoadedEvent>(onLevelLoaded);
        }
    }
}
