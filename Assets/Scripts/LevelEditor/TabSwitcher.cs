using UnityEngine;

namespace LevelEditor
{
    public class TabSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Transform tabContainer;

        public void SwitchTab(GameObject activeTab)
        {
            foreach (Transform child in tabContainer)
            {
                child.gameObject.SetActive(child.gameObject == activeTab);
            }
        }
    }
}
