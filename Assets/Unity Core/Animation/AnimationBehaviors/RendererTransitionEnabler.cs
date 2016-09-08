using UnityEngine;
using System.Collections;

public class RendererTransitionEnabler<Type> : SelectableTransitionBehaviour
    where Type : Renderer
{
    [SerializeField]
    private bool enable;

    [SerializeField]
    private bool delayForFrame;

    override protected void OnSetTransition(Animator animator)
    {
        var renderer = animator.gameObject.GetComponent<Type>();

        if (renderer != null)
        {
            GlobalState.Instance.RunCoroutine(Enable(renderer));
        }
    }

    private IEnumerator Enable(Type renderer)
    {
        if (delayForFrame)
        {
            yield return new WaitForEndOfFrame();
        }

        renderer.enabled = enable;
    }
}
