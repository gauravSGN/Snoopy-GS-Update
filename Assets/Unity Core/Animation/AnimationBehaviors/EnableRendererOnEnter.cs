using UnityEngine;

public class EnableRendererOnEnter<Type> : StateMachineBehaviour
    where Type : Renderer
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var renderer = animator.gameObject.GetComponent<Type>();

        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }
}
