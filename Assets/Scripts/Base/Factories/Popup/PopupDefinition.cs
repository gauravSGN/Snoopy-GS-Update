using Model;
using UnityEngine;

namespace UI.Popup
{
    public class PopupDefinition : ScriptableObject, GameObjectDefinition<PopupType>
    {
        public PopupType Type { get { return type; } }
        public GameObject Prefab { get { return prefab; } }
        public AudioClip DisplaySound { get { return displaySound; } }

        [SerializeField]
        private PopupType type;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private AudioClip displaySound;
    }
}
