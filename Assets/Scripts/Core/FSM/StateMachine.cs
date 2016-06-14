using UnityEngine;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        private State[] states;
        [SerializeField]
        private State startState;
        private State currentState;
        [SerializeField]
        private bool started = true;

        public bool Started
        {
            get{ return started; }
        }

        [System.Serializable]
        public class InterruptPair
        {
            public string interruptMessage;
            public State state;
        }

        [SerializeField]
        private List<InterruptPair> interrupts;

        #region Initialization and Start/Stop

        void Awake()
        {
            states = GetComponentsInChildren<FSM.State>();

            if(startState == null)
            {
                Debug.LogError("WARNING: FSM " + name + " does not specify a start state");
            }
        }

        public void StartRunning()
        {
            if(started)
            {
                Debug.LogWarning("Trying to start a state machine " + name + " when it is already running");
            }
            else
            {
                started = true;
                EnterState(startState);
            }
        }

        public void StopRunning()
        {
            started = false;
            currentState = startState;
        }

        #endregion

        #region Update and State Switching

        void Update()
        {
            if(started)
            {
                currentState.Tick(Time.deltaTime);
            }
        }

        void LateUpdate()
        {
            if(started)
            {
                Transition readyTransition = currentState.GetFirstReadyTransition();
                if(readyTransition != null)
                {
                    FireTransition(readyTransition);
                }
            }
        }

        private void FireTransition(Transition toFire)
        {
            State newState = toFire.Fire();
            SwitchToState(newState);
        }

        private void SwitchToState(State newState)
        {
            if(newState != null)
            {
                ExitCurrentState();
                EnterState(newState);
            }
        }

        private void EnterState(State newState)
        {
            currentState = newState;
            currentState.OnEnter();
        }

        private void ExitCurrentState()
        {
            if(currentState != null)
            {
                currentState.OnExit();
            }
        }

        #endregion

        #region MessageHandling

        public void FSMMessage(string message)
        {
            if(currentState != null)
            {
                currentState.HandleMessage(message);
            }
        }

        public void Interrupt(string message)
        {
            InterruptPair toUse = interrupts.Find(x => x.interruptMessage == message);
            if(toUse != null)
            {
                SwitchToState(toUse.state);
            }
        }

        #endregion
    }
}
