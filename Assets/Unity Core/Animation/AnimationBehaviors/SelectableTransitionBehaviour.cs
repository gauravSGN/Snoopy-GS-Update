using UnityEngine;

abstract public class SelectableTransitionBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private StateTransitionType transitionType;

    abstract protected void OnSetTransition(Animator animator);

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnTransition(animator, StateTransitionType.Enter);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnTransition(animator, StateTransitionType.Exit);
    }

    protected void OnTransition(Animator animator, StateTransitionType transition)
    {
        if ((transitionType == transition) || (transitionType == StateTransitionType.EnterAndExit))
        {
            OnSetTransition(animator);
        }
    }
}