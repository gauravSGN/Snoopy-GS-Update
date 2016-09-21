using Aiming;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimLineEventTrigger : EventTrigger
{
    public event Action<Vector2> MoveTarget;
    public event Action Fire;

    public bool Aiming { get { return aiming; } }

    private bool aiming;
    private bool hovering;

    public void Start()
    {
        GlobalState.EventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
    }

    #region Special Aiming Nonsense
    public void StartAiming()
    {
        StartAimingEvent.Dispatch();
    }

    public void StopAiming()
    {
        StopAimingEvent.Dispatch();
    }

    public void DoFire()
    {
        Fire();
        StopAimingEvent.Dispatch();
        aiming = false;
        hovering = false;
    }

    public void FakeDrag(Vector3 position)
    {
        Vector2 cursorPosition = new Vector2(position.x, position.y);
        MoveTarget(cursorPosition);
    }
    #endregion

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
            aiming = true;
            hovering = true;

            MoveTarget(GetCursorPosition(data));

            StartAimingEvent.Dispatch();
        }
    }

    override public void OnPointerEnter(PointerEventData data)
    {
        if (aiming && data.button == PointerEventData.InputButton.Left)
        {
            hovering = true;
            MoveTarget(GetCursorPosition(data));
            StartAimingEvent.Dispatch();
        }
    }

    override public void OnPointerExit(PointerEventData data)
    {
        if (aiming)
        {
            hovering = false;
            StopAimingEvent.Dispatch();
        }
    }

    override public void OnPointerUp(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if (hovering && aiming)
            {
                Fire();
            }

            StopAimingEvent.Dispatch();

            aiming = false;
            hovering = false;
        }
    }

    private Vector2 GetCursorPosition(PointerEventData data)
    {
        return data.pressEventCamera.ScreenToWorldPoint(data.pointerCurrentRaycast.screenPosition);
    }

    private void OnInputToggle(InputToggleEvent gameEvent)
    {
        if (aiming)
        {
            StopAimingEvent.Dispatch();

            aiming = false;
        }

        gameObject.SetActive(gameEvent.enabled);
    }
}
