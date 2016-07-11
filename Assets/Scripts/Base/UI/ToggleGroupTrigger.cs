using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ToggleGroupTrigger : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup toggleGroup;

    [TooltipAttribute("Indicates if any toggles are currently on")]
    [SerializeField]
    private bool togglesOn = false;

    [SpaceAttribute(10)]
    [SerializeField]
    private UnityEvent togglesOnCallback;

    [SerializeField]
    private UnityEvent togglesOffCallback;

    public void CheckToggles()
    {
        if (toggleGroup.AnyTogglesOn() != togglesOn)
        {
            togglesOn = !togglesOn;
            if (togglesOn)
            {
                togglesOnCallback.Invoke();
            }
            else
            {
                togglesOffCallback.Invoke();
            }
        }
    }
}
