using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods
{
    static public class MonoBehaviourExtensions
    {
        static public void StartCoroutinesSequential(this MonoBehaviour behaviour,
                                                     IEnumerable<IEnumerator> coroutines,
                                                     Action completeAction = null)
        {
            behaviour.StartCoroutine(RunSequential(coroutines, completeAction));
        }

        // Note that because we don't have access to Unity internals, this parallel version cannot properly handle
        // YieldInstructions like WaitForEndOfFrame or WaitForSeconds, so this should only be used for running
        // coroutines that yield return null or similar.
        static public void StartCoroutinesParallel(this MonoBehaviour behaviour,
                                                   IEnumerable<IEnumerator> coroutines,
                                                   Action completeAction = null)
        {
            behaviour.StartCoroutine(RunParallel(coroutines, completeAction));
        }

        static private IEnumerator RunSequential(IEnumerable<IEnumerator> coroutines, Action completeAction)
        {
            foreach (var coroutine in coroutines)
            {
                while (coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }

                yield return null;
            }

            if (completeAction != null)
            {
                completeAction.Invoke();
            }
        }

        static private IEnumerator RunParallel(IEnumerable<IEnumerator> coroutines, Action completeAction)
        {
            var coroutineList = coroutines.ToList();
            int index;

            while (coroutineList.Count > 0)
            {
                index = 0;

                while (index < coroutineList.Count)
                {
                    if (coroutineList[index].MoveNext())
                    {
                        index++;
                    }
                    else
                    {
                        coroutineList.RemoveAt(index);
                    }
                }

                yield return null;
            }

            if (completeAction != null)
            {
                completeAction.Invoke();
            }
        }
    }
}
