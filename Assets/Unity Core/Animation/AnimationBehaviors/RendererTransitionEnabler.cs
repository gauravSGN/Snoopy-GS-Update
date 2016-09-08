using UnityEngine;
using System.Collections;

public class RendererTransitionEnabler<Type> : SelectableTransitionBehaviour
    where Type : Renderer
{
    [SerializeField]
    private bool enable;

    override protected void OnSetTransition(Animator animator)
    {
        var renderer = animator.gameObject.GetComponent<Type>();

        if (renderer != null)
        {
            GlobalState.Instance.RunCoroutine(EnableEndOfFrame(renderer));
        }
    }

    private IEnumerator EnableEndOfFrame(Type renderer)
    {
        yield return new WaitForEndOfFrame();
        renderer.enabled = enable;
    }
}
