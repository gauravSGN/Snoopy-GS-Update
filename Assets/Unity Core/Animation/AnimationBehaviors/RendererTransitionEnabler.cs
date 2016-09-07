using UnityEngine;
using System.Collections;

public class RendererTransitionEnabler<Type> : BaseOnTransition
    where Type : Renderer
{
    [SerializeField]
    private bool enable;

    override protected void OnSetTransition(Animator animator)
    {
        var renderer = animator.gameObject.GetComponent<Type>();

        if (renderer != null)
        {
            GlobalState.Instance.StartCoroutine(EnableEndOfFrame(renderer));
            // renderer.enabled = enable;
        }
    }

    private IEnumerator EnableEndOfFrame(Type renderer)
    {
        yield return new WaitForEndOfFrame();
        renderer.enabled = enable;
    }
}
