using UnityEngine;
using System;
using System.Collections.Generic;

namespace FSM
{
    public class State : MonoBehaviour
    {
        public Color color = Color.white;
        private Transition[] outTransitions;
        private List<StateComponent> components;

        void Awake()
        {
            outTransitions = GetComponents<FSM.Transition>();
            Array.Sort(outTransitions, delegate(Transition t1, Transition t2) {
                return t1.priority.CompareTo(t2.priority);
            });
        }

        void Start()
        {
            MonoBehaviour[] comps = GetComponents<MonoBehaviour>();
            foreach (var b in comps)
            {
                if(b is StateComponent)
                {
                    components.Add((StateComponent)b);
                }
            }
        }

        public Transition GetFirstReadyTransition()
        {
            for(int i = 0; i < outTransitions.Length; ++i)
            {
                if(outTransitions[i].enabled && outTransitions[i].IsReady())
                {
                    return outTransitions[i];
                }
            }
            return null;
        }

        public void OnEnter()
        {
            for(int i = 0; i < components.Count; ++i)
            {
                components[i].OnEnter();
            }
            for(int i = 0; i < outTransitions.Length; ++i)
            {
                outTransitions[i].OnEnter();
            }
        }

        public void Tick(float deltaT)
        {
            for(int i = 0; i < components.Count; ++i)
            {
                components[i].Tick(deltaT);
            }
            for(int i = 0; i < outTransitions.Length; ++i)
            {
                outTransitions[i].Tick(deltaT);
            }
        }

        public void OnExit()
        {
            for(int i = 0; i < components.Count; ++i)
            {
                components[i].OnExit();
            }
            for(int i = 0; i < outTransitions.Length; ++i)
            {
                outTransitions[i].OnExit();
            }
        }

        public void HandleMessage(string message)
        {
            for(int i = 0; i < components.Count; ++i)
            {
                components[i].HandleMessage(message);
            }
            for(int i = 0; i < outTransitions.Length; ++i)
            {
                outTransitions[i].HandleMessage(message);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}