using Registry;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DisableSelectableWhenInputBlocked : MonoBehaviour
    {
        [SerializeField]
        private Selectable element;

        protected void Start()
        {
            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<InputToggleEvent>(OnInputToggled);
            eventService.AddEventHandler<BlockadeEvent.InputBlocked>(OnInputBlocked);
            eventService.AddEventHandler<BlockadeEvent.InputUnblocked>(OnInputUnblocked);
        }

        private void OnInputToggled(InputToggleEvent gameEvent)
        {
            element.interactable = gameEvent.enabled;
        }

        private void OnInputBlocked()
        {
            element.interactable = false;
        }

        private void OnInputUnblocked()
        {
            element.interactable = true;
        }
    }
}