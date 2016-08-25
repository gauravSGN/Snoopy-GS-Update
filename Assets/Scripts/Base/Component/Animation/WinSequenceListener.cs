using Sequence;
using UnityEngine;

public class WinSequenceListener : MonoBehaviour
{
    private Animator animator;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        GlobalState.EventService.AddEventHandler<StartWinAnimationsEvent>(OnStartWinAnimations);
    }

    private void OnStartWinAnimations(StartWinAnimationsEvent gameEvent)
    {
        if (animator != null)
        {
            animator.SetBool("WonLevel", true);
            GlobalState.EventService.RemoveEventHandler<StartWinAnimationsEvent>(OnStartWinAnimations);
        }
    }
}