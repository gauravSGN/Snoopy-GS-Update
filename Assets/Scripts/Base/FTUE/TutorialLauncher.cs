using UnityEngine;

namespace FTUE
{
    sealed public class TutorialLauncher : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlay;

        public void Start()
        {
            GlobalState.EventService.Persistent.AddEventHandler<ShowTutorialEvent>(OnShowTutorial);
        }

        private void OnShowTutorial(ShowTutorialEvent gameEvent)
        {
            overlay.SetActive(true);

            var path = string.Format("Tutorials/{0}", gameEvent.id);
            GlobalState.AssetService.LoadAssetAsync<GameObject>(path, OnTutorialLoaded);
        }

        private void OnTutorialLoaded(GameObject prefab)
        {
            Instantiate(prefab, transform);
            overlay.SetActive(false);
        }
    }
}
