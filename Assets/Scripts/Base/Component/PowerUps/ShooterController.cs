using UnityEngine;

namespace PowerUps
{
    public class ShooterController : MonoBehaviour
    {
        public void Start()
        {
            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<BubbleSettledEvent>(OnBubbleSettled);
            eventService.AddEventHandler<PowerUpChargeEvent>(OnCharge);
        }

        private void OnBubbleSettled(BubbleSettledEvent gameEvent)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

            var eventService = GlobalState.EventService;
            eventService.RemoveEventHandler<BubbleSettledEvent>(OnBubbleSettled);
            eventService.RemoveEventHandler<PowerUpChargeEvent>(OnCharge);

            Destroy(gameObject);
        }

        private void OnCharge(PowerUpChargeEvent gameEvent)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
