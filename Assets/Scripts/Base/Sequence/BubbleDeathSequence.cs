using Effects;
using Registry;
using Animation;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class BubbleDeathSequence : BlockingSequence
    {
        protected BubbleEffectController effectController;
        protected GameObject gameObject;

        public Dictionary<BubbleDeathType, List<IEnumerator>> EffectDictionary { get; private set; }

        override public BlockadeType BlockadeType { get { return BlockadeType.Reactions; } }

        public BubbleDeathSequence(GameObject gameObject) : base()
        {
            this.gameObject = gameObject;
            effectController = gameObject.GetComponent<BubbleEffectController>();
            EffectDictionary = new Dictionary<BubbleDeathType, List<IEnumerator>>();
        }

        public BubbleDeathSequence(GameObject gameObject, Dictionary<BubbleDeathType, List<IEnumerator>> effects)
            : base()
        {
            this.gameObject = gameObject;
            EffectDictionary = effects;
            effectController = gameObject.GetComponent<BubbleEffectController>();
        }

        virtual public void Play(BubbleDeathType type)
        {
            Play();

            var effects = EffectDictionary.ContainsKey(type) ? EffectDictionary[type] : GetDefaultEffects(type);

            foreach (var effect in effects)
            {
                effectController.AddEffect(effect);
            }
        }

        public void RegisterBlockers(GameObject blocker)
        {
            pending.Add(blocker);
        }

        public void AddEffect(GameObject parent, AnimationType type, BubbleDeathType deathType, bool blocking)
        {
            if (!EffectDictionary.ContainsKey(deathType))
            {
                EffectDictionary.Add(deathType, new List<IEnumerator>());
            }

            var effect = blocking ? AnimationEffect.PlayAndRegister(parent, type, RegisterBlockers) :
                                    AnimationEffect.Play(parent, type);

            EffectDictionary[deathType].Add(effect);
        }

        override protected void Complete(SequenceItemCompleteEvent gameEvent)
        {
            Complete();
        }

        protected void Complete()
        {
            if (gameObject.GetComponent<BubbleScore>().Score > 0)
            {
                effectController.AddEffect(AnimationEffect.Play(gameObject, AnimationType.ScoreText));
            }

            Object.Destroy(gameObject, 0.5f);
        }

        private List<IEnumerator> GetDefaultEffects(BubbleDeathType type)
        {
            var effects = new List<IEnumerator>();
            var model = gameObject.GetComponent<BubbleModelBehaviour>().Model;

            if (model.definition.AnimationMap.ContainsKey(type))
            {
                foreach (var animationType in model.definition.AnimationMap[type])
                {
                    effects.Add(AnimationEffect.PlayAndRegister(gameObject, animationType, RegisterBlockers));
                }
            }

            return effects;
        }
    }
}