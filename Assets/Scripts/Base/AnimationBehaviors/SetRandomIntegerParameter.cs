using UnityEngine;
using System.Collections.Generic;

public class SetRandomIntegerParameter : StateMachineBehaviour
{
    [SerializeField]
    private string parameterName;

    [SerializeField]
    private int minValue;

    [SerializeField]
    private int maxValue;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger(parameterName, Random.Range(minValue, maxValue));
    }
}
