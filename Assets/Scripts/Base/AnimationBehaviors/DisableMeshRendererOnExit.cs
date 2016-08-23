using UnityEngine;

public class DisableMeshRendererOnExit : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var renderer = animator.gameObject.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
}
