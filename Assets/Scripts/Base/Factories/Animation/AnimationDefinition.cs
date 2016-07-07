using UnityEngine;
using Model;

namespace Animation
{
    public class AnimationDefinition : ScriptableObject, GameObjectDefinition<AnimationType>
    {
        public AnimationType Type { get { return type; } }
        public GameObject Prefab { get { return prefab; } }
        public AnimatorOverrideController Controller { get { return controller; } }

        [SerializeField]
        private AnimationType type;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private AnimatorOverrideController controller;
    }
}
