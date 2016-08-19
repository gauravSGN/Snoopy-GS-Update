using Service;
using Registry;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.Director;

public class ParabolicTranslator : StateMachineBehaviour
{
    [SerializeField]
    private TransformUsage usage;

    private Transform transform;
    private Vector3 start;
    private Vector3 end;
    private Vector3 origin;
    private float coefficient;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.gameObject.transform;

        var targetTransform = GlobalState.Instance.Services.Get<TransformService>().Get(usage);
        start = transform.position;
        end = targetTransform.position;

        var first = 6.0f * start.x - 2.0f * end.x;
        var second = Mathf.Sqrt(12.0f) * (start.x - end.x);

        var candidate1 = (first + second) / 4.0f;
        var candidate2 = (first - second) / 4.0f;

        var xMid = (candidate1 > Mathf.Min(start.x, end.x)) ? candidate1 : candidate2;
        var yMid = start.y - (end.y - start.y) / 2.0f;

        origin = new Vector3(xMid, yMid);
        coefficient = (start.y - yMid) / ((start.x - xMid) * (start.x - xMid));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform.position = end;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var time = stateInfo.normalizedTime % 1.0f;
        var value = start.x + (end.x - start.x) * time;

        transform.position = new Vector3(value, origin.y + coefficient * (value - origin.x) * (value - origin.x));
    }
}
