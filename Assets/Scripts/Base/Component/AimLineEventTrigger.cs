using Aiming;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimLineEventTrigger : EventTrigger
{
    public event Action<Vector2> MoveTarget;
    public event Action Fire;

    private bool aiming;
    private bool hovering;

    public void Start()
    {
        GlobalState.EventService.AddEventHandler<InputToggleEvent>(OnInputToggle);
    }

    #region Special Aiming Nonsense
    protected void Update()
    {
        bool touching = Input.GetKey(KeyCode.Mouse0);
        if (!aiming && touching)
        {
            var rect = (transform as RectTransform);
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if point is inside the aim panel rect
            if (position.x >= corners[0].x && position.x <= corners[2].x && 
                position.y >= corners[0].y && position.y <= corners[2].y)
            {
                if (!hovering)
                {
                    hovering = true;
                    StartAimingEvent.Dispatch();
                }
                FakeDrag(position);
            }
            else if (hovering)
            {
                hovering = false;
                StopAimingEvent.Dispatch();
            }

        }
        else if (hovering && !touching)
        {
            Fire();
            hovering = false;
            StopAimingEvent.Dispatch();
        }
    }

    private void FakeDrag(Vector3 position)
    {
        Vector2 cursorPosition = new Vector2(position.x, position.y);
        MoveTarget(cursorPosition);
    }
    #endregion

    override public void OnDrag(PointerEventData data)
    {
        Debug.Log("On drag");
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
        Debug.Log("Pointer enter");
        if (aiming && data.button == PointerEventData.InputButton.Left)
        {
            hovering = true;
            MoveTarget(GetCursorPosition(data));
            StartAimingEvent.Dispatch();
        }
    }

    override public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("Pointer exit");
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
        Debug.Log("Screen pos = " + data.pointerCurrentRaycast.screenPosition);
        Debug.Log("World pos = " + data.pressEventCamera.ScreenToWorldPoint(data.pointerCurrentRaycast.screenPosition));
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
