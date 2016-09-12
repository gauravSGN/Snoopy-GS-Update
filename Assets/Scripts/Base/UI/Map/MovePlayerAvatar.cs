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

        [SerializeField]
        private GameObject unlockEffects;

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

            transform.position = new Vector3(destination.position.x, destination.position.y);
        }

        private void OnMovePlayerAvatar()
        {
            if (origin != null)
            {
                var effectsInstance = Instantiate(unlockEffects);
                effectsInstance.transform.SetParent(destination, false);

                Sound.PlaySoundEvent.Dispatch(Sound.SoundType.UnlockLevel);

                GoTween tween = transform.positionFrom(travelTime, origin.position);
                tween.easeCurve = easeCurve;
                tween.easeType = GoEaseType.AnimationCurve;
                tween.setOnCompleteHandler(OnAvatarMoveComplete);
            }
        }

        private void OnAvatarMoveComplete(AbstractGoTween tween)
        {
            var mapButtonComponent = destination.GetComponent<MapButton>();

            if (mapButtonComponent != null)
            {
                mapButtonComponent.Click(string.Empty);
            }
        }
    }
}