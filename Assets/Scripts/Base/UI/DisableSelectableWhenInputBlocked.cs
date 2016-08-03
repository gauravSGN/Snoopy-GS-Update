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
            GlobalState.EventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
        }

        private void OnInputToggle(InputToggleEvent gameEvent)
        {
            element.interactable = gameEvent.enabled;
        }
    }
}