﻿using UnityEngine;

namespace FSM
{
    public abstract class StateComponent : MonoBehaviour, StateLifeCycle
    {
        public virtual void OnEnter(){}

        public virtual void Tick(float deltaT){}

        public virtual void OnExit() {}

        public virtual void HandleMessage(string message) {}
    }
}