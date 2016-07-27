using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods
{
    static public class MonoBehaviourExtensions
    {
        static public void StartCoroutinesSequential(this MonoBehaviour behaviour, IEnumerable<IEnumerator> coroutines)
        {
            behaviour.StartCoroutine(RunSequential(coroutines));
        }

        static private IEnumerator RunSequential(IEnumerable<IEnumerator> coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                while (coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }

                yield return null;
            }
        }
    }
}
