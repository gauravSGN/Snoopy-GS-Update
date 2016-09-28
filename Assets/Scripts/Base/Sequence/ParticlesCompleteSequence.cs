using UnityEngine;
using System.Collections;

namespace Sequence
{
    public class ParticlesCompleteSequence : MonoBehaviour
    {
        private ParticleSystem[] particleSystems;

        public void Start()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
            StartCoroutine(CheckForComplete());
        }

        public IEnumerator CheckForComplete()
        {
            int index = 0;
            int count = particleSystems.Length;

            while (index < count)
            {
                if (particleSystems[index].IsAlive())
                {
                    yield return null;
                }
                else
                {
                    index++;
                }
            }

            GlobalState.EventService.Dispatch(new SequenceItemCompleteEvent(gameObject));
        }
    }
}