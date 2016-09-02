using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Animation
{
    sealed public class PreloadAnimations : MonoBehaviour
    {
        [Serializable]
        public struct AnimationPreloadInfo
        {
            public AnimationType type;
            public int count;
        }

        [SerializeField]
        private List<AnimationPreloadInfo> items;

        public void Start()
        {
            StartCoroutine(DoStuffNextFrame());
        }

        private IEnumerator DoStuffNextFrame()
        {
            yield return null;

            var animationService = GlobalState.AnimationService;

            foreach (var item in items)
            {
                animationService.Preload(item.type, item.count);
            }

            Destroy(gameObject);
        }
    }
}
