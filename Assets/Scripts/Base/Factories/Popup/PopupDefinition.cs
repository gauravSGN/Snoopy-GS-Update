using UnityEngine;
using Model;

namespace UI.Popup
{
    public class PopupDefinition : ScriptableObject, GameObjectDefinition<PopupType>
    {
        public PopupType Type { get { return type; } }
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        private PopupType type;

        [SerializeField]
        private GameObject prefab;
    }
}
