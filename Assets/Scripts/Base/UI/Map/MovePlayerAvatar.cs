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
        private Transform destination;

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<MovePlayerAvatarEvent>(OnMovePlayerAvatar);
            GlobalState.EventService.AddEventHandler<SetPlayerAvatarPositionEvent>(OnSetPlayerAvatarPosition);
        }

        private void OnSetPlayerAvatarPosition(SetPlayerAvatarPositionEvent gameEvent)
        {
            origin = gameEvent.origin;
            destination = gameEvent.destination;

            transform.SetParent(gameEvent.destination.parent, false);
            transform.localPosition = new Vector3(destination.localPosition.x,
                                                  destination.localPosition.y + yPadding);
        }

        private void OnMovePlayerAvatar()
        {
            if (origin != null)
            {
                GoTween tween = transform.localPositionFrom(travelTime, origin.localPosition);
                tween.easeCurve = easeCurve;
                tween.easeType = GoEaseType.AnimationCurve;
                tween.setOnCompleteHandler(OnAvatarMoveComplete);
            }
        }

        private void OnAvatarMoveComplete(AbstractGoTween tween)
        {
            Sound.PlaySoundEvent.Dispatch(Sound.SoundType.UnlockLevel);

            var mapButtonComponent = destination.GetComponent<MapButton>();

            if (mapButtonComponent != null)
            {
                mapButtonComponent.Click(string.Empty);
            }
        }
    }
}