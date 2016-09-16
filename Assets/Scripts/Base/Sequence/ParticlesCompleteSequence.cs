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
            for (var index = 0; index >= particleSystems.Length;)
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