using System;
using Service;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimLineEventTrigger : EventTrigger
{
    public event Action StartAiming;
    public event Action StopAiming;
    public event Action<Vector2> MoveTarget;
    public event Action Fire;

    private bool aiming;
    private bool hovering;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<InputToggleEvent>(OnInputToggle);
    }

    override public void OnDrag(PointerEventData data)
    {
        if (hovering)
        {
            MoveTarget(GetCursorPosition(data));
        }
    }

    override public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            aiming = hovering = true;

            MoveTarget(GetCursorPosition(data));
            StartAiming();
        }
    }

    override public void OnPointerEnter(PointerEventData data)
    {
        if (aiming && data.button == PointerEventData.InputButton.Left)
        {
            hovering = true;
            MoveTarget(GetCursorPosition(data));
            StartAiming();
        }
    }

    override public void OnPointerExit(PointerEventData data)
    {
        if (aiming)
        {
            hovering = false;
            StopAiming();
        }
    }

    override public void OnPointerUp(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if (hovering)
            {
                Fire();
            }

            StopAiming();

            aiming = hovering = false;
        }
    }

    private Vector2 GetCursorPosition(PointerEventData data)
    {
        return data.pressEventCamera.ScreenToWorldPoint(data.pointerCurrentRaycast.screenPosition);
    }

    private void OnInputToggle(InputToggleEvent gameEvent)
    {
        StopAiming();

        gameObject.SetActive(gameEvent.enabled);
    }
}
