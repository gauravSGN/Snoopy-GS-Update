using Util;
using UnityEngine;
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

        public void Preload(AnimationType type, int count)
        {
            var definition = GetDefinitionByType(type);

            animationPool.Allocate(definition.Prefab, count, (prefab) =>
            {
                var instance = animationPool.DefaultAllocator(prefab);
                HidePooledObject(instance);
                return instance;
            });
        }

        override public GameObject CreateByType(AnimationType type)
        {
            var definition = GetDefinitionByType(type);
            var instance = animationPool.Get(definition.Prefab);

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

            GlobalState.EventService.Persistent.AddEventHandler<AnimationCompleteEvent>(OnAnimationComplete);
        }

        private void OnAnimationComplete(AnimationCompleteEvent gameEvent)
        {
            var instance = gameEvent.gameObject;
            HidePooledObject(instance);

            animationPool.Release(instance);
        }

        private void HidePooledObject(GameObject instance)
        {
            instance.transform.SetParent(parent, false);
        }
    }
}
