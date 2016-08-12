using UnityEngine;

namespace UI.Map
{
    public class MovePlayerAvatar : MonoBehaviour
    {
        [SerializeField]
        private float yPadding;

        [SerializeField]
        private float travelTime;

        [SerializeField]
        private AnimationCurve easeCurve;

        private Transform origin;

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<MovePlayerAvatarEvent>(OnMovePlayerAvatar);
            GlobalState.EventService.AddEventHandler<SetPlayerAvatarPositionEvent>(OnSetPlayerAvatarPosition);
        }

        private void OnSetPlayerAvatarPosition(SetPlayerAvatarPositionEvent gameEvent)
        {
            origin = gameEvent.origin;

            transform.SetParent(gameEvent.destination.parent, false);
            transform.localPosition = new Vector3(gameEvent.destination.localPosition.x,
                                                  gameEvent.destination.localPosition.y + yPadding);
        }

        private void OnMovePlayerAvatar(MovePlayerAvatarEvent gameEvent)
        {
            if (origin != null)
            {
                GoTween tween = transform.localPositionFrom(travelTime, origin.localPosition);
                tween.easeCurve = easeCurve;
                tween.easeType = GoEaseType.AnimationCurve;
            }
        }
    }
}