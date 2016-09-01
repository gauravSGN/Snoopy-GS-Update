using Util;
using UnityEngine;
using Event.Animation;
using UnityEngine.SceneManagement;

namespace Animation
{
    public class AnimationFactory : ScriptableFactory<AnimationType, AnimationDefinition>
    {
        private readonly GameObjectPool animationPool = new GameObjectPool();
        private Transform parent;

        public AnimationFactory()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        override public GameObject CreateByType(AnimationType type)
        {
            var definition = GetDefinitionByType(type);
            var instance = animationPool.Get(definition.Prefab);
            instance.SetActive(true);

            if (definition.Controller != null)
            {
                instance.GetComponent<Animator>().runtimeAnimatorController = definition.Controller;
            }

            return instance;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            animationPool.Clear();

            var poolObject = new GameObject();
            poolObject.name = "PooledObjects";
            poolObject.SetActive(false);

            parent = poolObject.transform;

            GlobalState.EventService.AddEventHandler<AnimationCompleteEvent>(OnAnimationComplete);
        }

        private void OnAnimationComplete(AnimationCompleteEvent gameEvent)
        {
            var instance = gameEvent.gameObject;

            instance.transform.SetParent(parent, false);
            instance.SetActive(false);

            animationPool.Release(instance);
        }
    }
}
