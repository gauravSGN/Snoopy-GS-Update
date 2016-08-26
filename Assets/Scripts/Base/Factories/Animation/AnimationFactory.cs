using UnityEngine;
using Util;

namespace Animation
{
    public class AnimationFactory : ScriptableFactory<AnimationType, AnimationDefinition>
    {
        override public GameObject CreateByType(AnimationType type)
        {
            var definition = GetDefinitionByType(type);
            var instance = Instantiate(definition.Prefab);

            if (definition.Controller != null)
            {
                instance.GetComponent<Animator>().runtimeAnimatorController = definition.Controller;
            }

            return instance;
        }
    }
}
