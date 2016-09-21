using UnityEngine;
using Sequence;

sealed public class InputStatus : MonoBehaviour
{
    public bool Enabled { get; private set; }

    public void Start()
    {
        Enabled = true;

        GlobalState.EventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
        GlobalState.EventService.AddEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);
    }

    private void OnInputToggle(InputToggleEvent gameEvent)
    {
        Enabled = gameEvent.enabled;
    }

    private void OnPrepareForBubbleParty()
    {
        GlobalState.EventService.RemoveEventHandler<InputToggleEvent>(OnInputToggle);
        GlobalState.EventService.RemoveEventHandler<PrepareForBubblePartyEvent>(OnPrepareForBubbleParty);

        Enabled = false;
    }
}
